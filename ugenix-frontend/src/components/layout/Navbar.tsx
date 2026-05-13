import React from 'react';
import { NavLink } from 'react-router-dom';
import { Map, Ticket, User, Sparkles } from 'lucide-react';
import { clsx } from 'clsx';
import { ActiveOverlay } from '../../store/ui.store';

interface NavbarProps {
  onOpenOverlay: (type: ActiveOverlay) => void;
}

interface NavItem {
  to?: string;
  type?: ActiveOverlay;
  icon: any;
  label: string;
}

export default function Navbar({ onOpenOverlay }: NavbarProps) {
  const navItems: NavItem[] = [
    { to: '/discovery', icon: Map, label: 'Discovery' },
    { to: '/marketplace', icon: Ticket, label: 'Marketplace' },
    { to: '/profile', icon: User, label: 'Profile' },
  ];

  return (
    <nav className="sticky top-0 z-50 w-full border-b border-white/5 bg-background/80 backdrop-blur-xl">
      <div className="max-w-7xl mx-auto px-6 h-16 flex items-center justify-between">
        <NavLink to="/" className="flex items-center gap-2 group">
          <div className="w-8 h-8 bg-indigo-600 rounded-lg flex items-center justify-center group-hover:rotate-12 transition-transform">
            <Sparkles className="w-5 h-5 text-white" />
          </div>
          <span className="font-bold text-xl tracking-tight text-white">UGenix</span>
        </NavLink>

        <div className="hidden md:flex items-center gap-8">
          {navItems.map((item) => (
            item.to ? (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) => clsx(
                  "flex items-center gap-2 text-sm font-medium transition-colors hover:text-indigo-400",
                  isActive ? "text-indigo-400" : "text-gray-400"
                )}
              >
                <item.icon className="w-4 h-4" />
                {item.label}
              </NavLink>
            ) : (
              <button
                key={item.type}
                onClick={() => onOpenOverlay(item.type as ActiveOverlay)}
                className="flex items-center gap-2 text-sm font-medium transition-colors text-gray-400 hover:text-indigo-400"
              >
                <item.icon className="w-4 h-4" />
                {item.label}
              </button>
            )
          ))}
        </div>

        <button 
          onClick={() => onOpenOverlay('auth')}
          className="btn-primary py-1.5 px-4 text-xs"
        >
          Connect Wallet
        </button>
      </div>
    </nav>
  );
}
