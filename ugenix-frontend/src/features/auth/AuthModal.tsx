import React, { useState } from 'react';
import { Mail, Lock, User, ArrowRight, Github } from 'lucide-react';
import Modal from '../../components/ui/Modal';
import { authApi } from './auth.api';
import { useAuthStore } from '../../store/auth.store';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';

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
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const response = (err as { response?: { data?: { title?: string } } }).response;
        setError(response?.data?.title || 'Authentication failed. Please try again.');
      } else {
        setError('Authentication failed. Please try again.');
      }
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
        <p className="text-slate-400 text-sm">
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
            <div className="relative group">
              <User className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-500 group-focus-within:text-violet-400 transition-colors z-10" />
              <Input label="Full Name" placeholder="John Doe" className="pl-12" />
            </div>
          )}

          <div className="relative group">
            <Mail className="absolute left-4 top-[36px] -translate-y-1/2 w-4 h-4 text-slate-500 group-focus-within:text-violet-400 transition-colors z-10" />
            <Input 
              label="Email Address"
              type="email" 
              placeholder="name@example.com" 
              className="pl-12" 
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="relative group">
            <Lock className="absolute left-4 top-[36px] -translate-y-1/2 w-4 h-4 text-slate-500 group-focus-within:text-violet-400 transition-colors z-10" />
            <Input 
              label="Password"
              type="password" 
              placeholder="••••••••" 
              className="pl-12" 
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <Button 
            type="submit" 
            isLoading={isLoading}
            className="w-full mt-2"
          >
            <div className="flex items-center gap-2 group">
              <span>{mode === 'login' ? 'Sign In' : 'Create Account'}</span>
              {!isLoading && <ArrowRight className="w-4 h-4 group-hover:translate-x-1 transition-transform" />}
            </div>
          </Button>
        </form>

        <div className="relative py-2">
          <div className="absolute inset-0 flex items-center"><div className="w-full border-t border-slate-800/50"></div></div>
          <div className="relative flex justify-center text-xs uppercase"><span className="bg-surface px-2 text-slate-500">Or continue with</span></div>
        </div>

        <Button variant="secondary" className="w-full gap-3">
          <Github className="w-5 h-5" />
          <span>GitHub</span>
        </Button>

        <p className="text-center text-sm text-slate-400">
          {mode === 'login' ? "Don't have an account?" : "Already have an account?"}{' '}
          <button onClick={toggleMode} className="text-violet-400 font-bold hover:underline">
            {mode === 'login' ? 'Sign Up' : 'Log In'}
          </button>
        </p>
      </div>
    </Modal>
  );
}
