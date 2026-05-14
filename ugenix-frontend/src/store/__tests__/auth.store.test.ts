import { describe, it, expect, beforeEach } from 'vitest';
import { useAuthStore } from '../auth.store';

describe('Auth Store', () => {
  beforeEach(() => {
    // Reset store before each test
    useAuthStore.setState({ user: null, isAuthenticated: false, accessToken: null });
  });

  it('should initialize with default state', () => {
    const state = useAuthStore.getState();
    expect(state.user).toBeNull();
    expect(state.isAuthenticated).toBe(false);
  });

  it('should set credentials', () => {
    const mockUser = { id: '1', email: 'test@example.com', name: 'Test User', role: 'User' };
    useAuthStore.getState().setCredentials(mockUser);
    
    const state = useAuthStore.getState();
    expect(state.user).toEqual(mockUser);
    expect(state.isAuthenticated).toBe(true);
  });

  it('should clear credentials on logout', () => {
    const mockUser = { id: '1', email: 'test@example.com', name: 'Test User', role: 'User' };
    useAuthStore.getState().setCredentials(mockUser);
    useAuthStore.getState().logout();
    
    const state = useAuthStore.getState();
    expect(state.user).toBeNull();
    expect(state.isAuthenticated).toBe(false);
  });
});
