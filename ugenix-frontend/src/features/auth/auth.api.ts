import api from '../../api/client';

export interface UserDto {
  id: string;
  email: string;
  role: string;
}

export interface AuthResponse {
  accessToken: string;
  user: UserDto;
}

export const authApi = {
  login: async (email: string, password: string): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/v1/identity/login', { email, password });
    return response as unknown as AuthResponse;
  },
  
  register: async (email: string, password: string): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/v1/identity/register', { email, password });
    return response as unknown as AuthResponse;
  }
};
