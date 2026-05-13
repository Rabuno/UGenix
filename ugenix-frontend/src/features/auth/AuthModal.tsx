import React, { useState } from 'react';
import { Mail, Lock, User, ArrowRight, Github } from 'lucide-react';
import Modal from '../../components/ui/Modal';

type AuthMode = 'login' | 'register';

interface AuthModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export default function AuthModal({ isOpen, onClose }: AuthModalProps) {
  const [mode, setMode] = useState<AuthMode>('login');
  const toggleMode = () => setMode(mode === 'login' ? 'register' : 'login');

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

        <form className="space-y-4">
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
              <input type="email" placeholder="name@example.com" className="input-field pl-12" />
            </div>
          </div>

          <div className="space-y-1.5">
            <label className="text-xs font-semibold uppercase tracking-wider text-gray-500 ml-1">Password</label>
            <div className="relative group">
              <Lock className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 group-focus-within:text-indigo-400 transition-colors" />
              <input type="password" placeholder="••••••••" className="input-field pl-12" />
            </div>
          </div>

          <button type="button" className="btn-primary w-full py-3 flex items-center justify-center gap-2 group">
            <span>{mode === 'login' ? 'Sign In' : 'Create Account'}</span>
            <ArrowRight className="w-4 h-4 group-hover:translate-x-1 transition-transform" />
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
