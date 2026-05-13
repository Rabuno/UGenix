import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuthStore } from '../store/auth.store';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

/**
 * Higher-Order Component to protect routes.
 * Redirects to home page with auth overlay trigger if not authenticated.
 */
export default function ProtectedRoute({ children }: ProtectedRouteProps) {
  const { isAuthenticated } = useAuthStore();
  const location = useLocation();

  if (!isAuthenticated) {
    // Redirect to home and trigger the auth modal via query param
    return <Navigate to="/?overlay=auth" state={{ from: location }} replace />;
  }

  return <>{children}</>;
}
