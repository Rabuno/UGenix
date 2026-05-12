import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import App from './App';
import './index.css';
import { loadRuntimeConfig, validateEnv } from './config/env.config';

// 1. Initialize Query Client with production-grade defaults
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
      staleTime: 1000 * 60 * 5, // 5 minutes
    },
  },
});

/**
 * Application Bootstrapper
 * Ensures environment is valid and runtime config is loaded before mounting.
 */
const bootstrap = async () => {
  // Load dynamic config (CDN injection)
  await loadRuntimeConfig();
  
  // Fail-fast if critical keys are missing
  validateEnv();

  ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <QueryClientProvider client={queryClient}>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </QueryClientProvider>
    </React.StrictMode>
  );
};

bootstrap();
