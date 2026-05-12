import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';

interface User {
  id: string;
  email: string;
  name: string;
  role: string;
}

interface AuthState {
  user: User | null;
  accessToken: string | null;
  isAuthenticated: boolean;
  setAuth: (user: User, token: string) => void;
  clearAuth: () => void;
  updateAccessToken: (token: string) => void;
}

/**
 * Auth Store using Zustand.
 * Persists basic user info to sessionStorage for refresh safety,
 * while keeping sensitive access tokens available in memory.
 */
export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      user: null,
      accessToken: null,
      isAuthenticated: false,
      setAuth: (user, token) => set({ user, accessToken: token, isAuthenticated: true }),
      clearAuth: () => set({ user: null, accessToken: null, isAuthenticated: false }),
      updateAccessToken: (token) => set({ accessToken: token }),
    }),
    {
      name: 'ugenix-auth-storage',
      storage: createJSONStorage(() => sessionStorage),
      partialize: (state) => ({ user: state.user, isAuthenticated: state.isAuthenticated }),
    }
  )
);

