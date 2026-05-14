import React from 'react';
import { NavLink } from 'react-router-dom';
import { Map, Ticket, User, Sparkles } from 'lucide-react';
import { clsx } from 'clsx';
import { ActiveOverlay } from '../../store/ui.store';
import { useAuthStore } from '../../store/auth.store';
import { Button } from '../ui/Button';

interface NavbarProps {
  onOpenOverlay: (type: ActiveOverlay) => void;
}

interface NavItem {
  to?: string;
  type?: ActiveOverlay;
  icon: React.ElementType;
  label: string;
}

export default function Navbar({ onOpenOverlay }: NavbarProps) {
  const { user, isAuthenticated, clearAuth } = useAuthStore();
  
  const navItems: NavItem[] = [
    { to: '/discovery', icon: Map, label: 'Discovery' },
    { to: '/marketplace', icon: Ticket, label: 'Marketplace' },
    { to: '/profile', icon: User, label: 'Profile' },
  ];

  return (
    <nav className="sticky top-0 z-nav w-full border-b border-slate-800/50 bg-background/80 backdrop-blur-xl">
      <div className="max-w-7xl mx-auto px-6 h-16 flex items-center justify-between">
        <NavLink to="/" className="flex items-center gap-2 group">
          <div className="w-8 h-8 bg-amethyst-gradient rounded-lg flex items-center justify-center group-hover:rotate-12 transition-transform shadow-lg shadow-violet-500/20">
            <Sparkles className="w-5 h-5 text-white" />
          </div>
          <span className="font-bold text-xl tracking-tight text-slate-50">UGenix</span>
        </NavLink>

        <div className="hidden md:flex items-center gap-8">
          {navItems.map((item) => (
            item.to ? (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) => clsx(
                  "flex items-center gap-2 text-sm font-medium transition-colors hover:text-violet-400",
                  isActive ? "text-violet-400" : "text-slate-400"
                )}
              >
                <item.icon className="w-4 h-4" />
                {item.label}
              </NavLink>
            ) : (
              <button
                key={item.type}
                onClick={() => onOpenOverlay(item.type as ActiveOverlay)}
                className="flex items-center gap-2 text-sm font-medium transition-colors text-slate-400 hover:text-violet-400"
              >
                <item.icon className="w-4 h-4" />
                {item.label}
              </button>
            )
          ))}
        </div>

        <div className="flex items-center gap-4">
          {isAuthenticated ? (
            <div className="flex items-center gap-4">
              <span className="text-xs text-slate-400 font-medium hidden sm:inline-block">{user?.email}</span>
              <button 
                onClick={() => clearAuth()}
                className="text-xs font-bold text-red-400 hover:text-red-300 transition-colors"
              >
                Sign Out
              </button>
            </div>
          ) : (
            <Button 
              onClick={() => onOpenOverlay('auth')}
              size="sm"
            >
              Sign In
            </Button>
          )}
        </div>
      </div>
    </nav>
  );
}
