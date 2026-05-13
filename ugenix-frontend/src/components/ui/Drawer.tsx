import React, { useEffect } from 'react';
import { X } from 'lucide-react';
import Portal from './Portal';
import { clsx } from 'clsx';

interface DrawerProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
}

export default function Drawer({ isOpen, onClose, title, children }: DrawerProps) {
  useEffect(() => {
    const handleEscape = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };

    if (isOpen) {
      document.addEventListener('keydown', handleEscape);
      document.body.style.overflow = 'hidden';
    }

    return () => {
      document.removeEventListener('keydown', handleEscape);
      document.body.style.overflow = 'unset';
    };
  }, [isOpen, onClose]);

  return (
    <Portal>
      <div 
        className={clsx(
          "fixed inset-0 z-[100] transition-opacity duration-300",
          isOpen ? "opacity-100 pointer-events-auto" : "opacity-0 pointer-events-none"
        )}
      >
        {/* Backdrop */}
        <div 
          className="absolute inset-0 bg-background/60 backdrop-blur-sm"
          onClick={onClose}
        />
        
        {/* Drawer Container */}
        <div 
          role="dialog"
          aria-modal="true"
          className={clsx(
            "absolute right-0 top-0 h-full w-full sm:w-[450px] bg-surface border-l border-white/5 shadow-2xl transition-transform duration-300 ease-out flex flex-col",
            isOpen ? "translate-x-0" : "translate-x-full"
          )}
        >
          {/* Header */}
          <div className="h-16 flex items-center justify-between px-6 border-b border-white/5 shrink-0">
            <h2 className="text-xl font-bold">{title}</h2>
            <button 
              onClick={onClose}
              className="p-2 hover:bg-white/5 rounded-full transition-colors text-gray-400 hover:text-white"
            >
              <X className="w-5 h-5" />
            </button>
          </div>
          
          {/* Content */}
          <div className="flex-1 overflow-y-auto custom-scrollbar p-6">
            {children}
          </div>
        </div>
      </div>
    </Portal>
  );
}
