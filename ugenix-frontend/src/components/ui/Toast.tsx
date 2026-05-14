import React from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { X, CheckCircle2, AlertCircle, Info } from 'lucide-react';
import { clsx } from 'clsx';
import { twMerge } from 'tailwind-merge';

// A simple UI component for Toasts to demonstrate the design system.
// In a real app, this would be managed by a global store (Zustand) or a library like Sonner.

export interface ToastProps {
  id: string;
  title: string;
  description?: string;
  type?: 'success' | 'error' | 'info';
  onClose: (id: string) => void;
}

export function Toast({ id, title, description, type = 'info', onClose }: ToastProps) {
  const icons = {
    success: <CheckCircle2 className="w-5 h-5 text-green-400" />,
    error: <AlertCircle className="w-5 h-5 text-red-400" />,
    info: <Info className="w-5 h-5 text-violet-400" />
  };

  const borders = {
    success: 'border-green-500/20',
    error: 'border-red-500/20',
    info: 'border-violet-500/20'
  };

  return (
    <motion.div
      layout
      initial={{ opacity: 0, y: 50, scale: 0.9 }}
      animate={{ opacity: 1, y: 0, scale: 1 }}
      exit={{ opacity: 0, scale: 0.9, transition: { duration: 0.2 } }}
      className={twMerge(
        clsx(
          "pointer-events-auto flex w-full max-w-md rounded-lg bg-surface border shadow-lg shadow-black/50 overflow-hidden z-toast",
          borders[type]
        )
      )}
    >
      <div className="flex-1 p-4 flex gap-3 items-start">
        <div className="shrink-0 mt-0.5">{icons[type]}</div>
        <div className="flex-1 space-y-1">
          <p className="text-sm font-semibold text-slate-50">{title}</p>
          {description && <p className="text-sm text-slate-400">{description}</p>}
        </div>
      </div>
      <button 
        onClick={() => onClose(id)}
        className="p-4 text-slate-500 hover:text-slate-300 bg-slate-800/10 hover:bg-slate-800/30 transition-colors border-l border-slate-800/50"
      >
        <X className="w-4 h-4" />
      </button>
    </motion.div>
  );
}

export function ToastContainer({ toasts, onClose }: { toasts: ToastProps[], onClose: (id: string) => void }) {
  return (
    <div className="fixed bottom-0 right-0 z-toast p-4 md:p-6 space-y-4 w-full md:max-w-sm pointer-events-none flex flex-col items-end">
      <AnimatePresence>
        {toasts.map(toast => (
          <Toast key={toast.id} {...toast} onClose={onClose} />
        ))}
      </AnimatePresence>
    </div>
  );
}
