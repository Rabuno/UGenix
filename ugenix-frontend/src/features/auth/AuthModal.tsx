import React, { useState } from 'react';
import { Mail, Lock, User, ArrowRight, Github, Loader2 } from 'lucide-react';
import Modal from '../../components/ui/Modal';
import { authApi } from './auth.api';
import { useAuthStore } from '../../store/auth.store';

type AuthMode = 'login' | 'register';

interface AuthModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export default function AuthModal({ isOpen, onClose }: AuthModalProps) {
  const [mode, setMode] = useState<AuthMode>('login');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const setAuth = useAuthStore((state) => state.setAuth);

  const toggleMode = () => {
    setMode(mode === 'login' ? 'register' : 'login');
    setError(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    try {
      const response = mode === 'login' 
        ? await authApi.login(email, password)
        : await authApi.register(email, password);
      
      setAuth({
        id: response.user.id,
        email: response.user.email,
        name: response.user.email.split('@')[0], // Mock name
        role: response.user.role
      }, response.accessToken);
      
      onClose();
    } catch (err: any) {
      setError(err.response?.data?.title || 'Authentication failed. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal 
      isOpen={isOpen} 
      onClose={onClose} 
      title={mode === 'login' ? 'Welcome Back' : 'Join UGenix'}
    >
      <div className="space-y-6">
        <p className="text-gray-400 text-sm">
          {mode === 'login' 
            ? 'Access your discovery dashboard and saved vouchers.' 
            : 'Start your premium discovery journey with us today.'}
        </p>

        {error && (
          <div className="p-3 rounded-lg bg-red-500/10 border border-red-500/20 text-red-400 text-xs font-medium">
            {error}
          </div>
        )}

        <form className="space-y-4" onSubmit={handleSubmit}>
          {mode === 'register' && (
            <div className="space-y-1.5">
              <label className="text-xs font-semibold uppercase tracking-wider text-gray-500 ml-1">Full Name</label>
              <div className="relative group">
                <User className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 group-focus-within:text-indigo-400 transition-colors" />
                <input type="text" placeholder="John Doe" className="input-field pl-12" />
              </div>
            </div>
          )}

          <div className="space-y-1.5">
            <label className="text-xs font-semibold uppercase tracking-wider text-gray-500 ml-1">Email Address</label>
            <div className="relative group">
              <Mail className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 group-focus-within:text-indigo-400 transition-colors" />
              <input 
                type="email" 
                placeholder="name@example.com" 
                className="input-field pl-12" 
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
          </div>

          <div className="space-y-1.5">
            <label className="text-xs font-semibold uppercase tracking-wider text-gray-500 ml-1">Password</label>
            <div className="relative group">
              <Lock className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 group-focus-within:text-indigo-400 transition-colors" />
              <input 
                type="password" 
                placeholder="••••••••" 
                className="input-field pl-12" 
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
          </div>

          <button 
            type="submit" 
            disabled={isLoading}
            className="btn-primary w-full py-3 flex items-center justify-center gap-2 group disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {isLoading ? (
              <Loader2 className="w-4 h-4 animate-spin" />
            ) : (
              <>
                <span>{mode === 'login' ? 'Sign In' : 'Create Account'}</span>
                <ArrowRight className="w-4 h-4 group-hover:translate-x-1 transition-transform" />
              </>
            )}
          </button>
        </form>

        <div className="relative py-2">
          <div className="absolute inset-0 flex items-center"><div className="w-full border-t border-white/5"></div></div>
          <div className="relative flex justify-center text-xs uppercase"><span className="bg-surface px-2 text-gray-500">Or continue with</span></div>
        </div>

        <button className="w-full py-3 glass-card flex items-center justify-center gap-3 hover:bg-white/5 transition-all">
          <Github className="w-5 h-5" />
          <span className="text-sm font-semibold">GitHub</span>
        </button>

        <p className="text-center text-sm text-gray-400">
          {mode === 'login' ? "Don't have an account?" : "Already have an account?"}{' '}
          <button onClick={toggleMode} className="text-indigo-400 font-bold hover:underline">
            {mode === 'login' ? 'Sign Up' : 'Log In'}
          </button>
        </p>
      </div>
    </Modal>
  );
}
