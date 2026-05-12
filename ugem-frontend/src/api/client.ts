import axios, { AxiosError, AxiosInstance, InternalAxiosRequestConfig } from 'axios';
import { ENV } from '../config/env.config';
import { useAuthStore } from '../store/auth.store';

/**
 * Standard API Response Envelope matching Backend ApiEnvelope<T>
 */
export interface ApiEnvelope<T> {
  data: T;
  meta: {
    traceId: string;
    correlationId: string;
    timestamp: string;
    version: string;
  };
}

/**
 * RFC7807 Problem Details
 */
export interface ProblemDetails {
  type: string;
  title: string;
  status: number;
  detail: string;
  instance: string;
  errors?: Record<string, string[]>;
}

const api: AxiosInstance = axios.create({
  baseURL: ENV.API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
});

// Variables for Token Refresh Mutex/Queue
let isRefreshing = false;
let failedQueue: Array<{ resolve: (token: string) => void; reject: (error: any) => void }> = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token!);
    }
  });
  failedQueue = [];
};

// Request Interceptor: Traceability & Auth
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // 1. Traceability
    const correlationId = crypto.randomUUID();
    config.headers['X-Correlation-Id'] = correlationId;

    // 2. Auth (Access Token from Memory Store)
    const token = useAuthStore.getState().accessToken;
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

// Response Interceptor: Unwrapping & Error Handling
api.interceptors.response.use(
  (response) => {
    // Automatic unwrapping of ApiEnvelope
    return response.data;
  },
  async (error: AxiosError<ProblemDetails>) => {
    const originalRequest = error.config as any;

    // Handle 401 Unauthorized (Token Refresh Mutex Logic)
    if (error.response?.status === 401 && !originalRequest._retry) {
      
      if (isRefreshing) {
        // Queue the request until refresh is complete
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers.Authorization = `Bearer ${token}`;
            return api(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        // Attempt Refresh via HttpOnly Cookie (POST /v1/identity/refresh)
        // Note: Using base axios to avoid infinite loop
        const { data } = await axios.post(
          `${ENV.API_BASE_URL}/v1/identity/refresh`, 
          {}, 
          { withCredentials: true, timeout: 10000 }
        );
        const { accessToken, user } = data.data;

        // Update Store
        useAuthStore.getState().setAuth(user, accessToken);
        
        // Update Default Headers
        api.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        
        processQueue(null, accessToken);
        return api(originalRequest);
      } catch (refreshError) {
        processQueue(refreshError, null);
        useAuthStore.getState().clearAuth();
        window.location.href = '/login?expired=true';
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    // Standardized Error Parsing (RFC7807)
    const problem = error.response?.data;
    if (problem && problem.title) {
      console.error(`[API Error] ${problem.title}: ${problem.detail}`, problem.errors);
    }

    return Promise.reject(error);
  }
);

export default api;
