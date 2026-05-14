# UGenix UI/UX Reconstruction - Phase 1: Foundation (Design System) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Establish the core Design System foundations including Tailwind configuration, CSS variables, and reusable UI components following the "Elegant Amethyst" theme.

**Architecture:** Atomic Design-inspired component architecture. Styles are managed via Tailwind CSS with a central theme configuration. Global styles and interaction tokens are defined via CSS variables in `index.css`.

**Tech Stack:** React 18, TypeScript, Tailwind CSS, Lucide React (Icons), Framer Motion (Animations).

---

### Task 1: Update Tailwind Configuration

**Files:**
- Modify: `ugenix-frontend/tailwind.config.js`

- [ ] **Step 1: Add "Amethyst" color palette and extended theme settings**

```javascript
/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        background: "#020617", // Slate-950
        surface: "#0f172a",    // Slate-900
        primary: {
          DEFAULT: "#6d28d9",  // Violet-700
          foreground: "#f8fafc",
        },
        secondary: {
          DEFAULT: "#1e293b",  // Slate-800
          foreground: "#e2e8f0",
        },
        accent: {
          DEFAULT: "#8b5cf6",  // Violet-500
          glow: "rgba(139, 92, 246, 0.4)",
        },
        violet: {
          700: "#6d28d9",
          800: "#5b21b6",
          900: "#4c1d95",
        }
      },
      borderRadius: {
        'button': '10px',
        'card': '16px',
      },
      zIndex: {
        'nav': '40',
        'sticky': '30',
        'drawer': '50',
        'modal': '60',
        'toast': '70',
      },
      backgroundImage: {
        'amethyst-gradient': 'linear-gradient(135deg, #6d28d9 0%, #4c1d95 100%)',
      }
    },
  },
  plugins: [],
};
```

- [ ] **Step 2: Commit changes**

```bash
git add ugenix-frontend/tailwind.config.js
git commit -m "style: update tailwind config with amethyst tokens"
```

---

### Task 2: Define Global CSS Variables and Interaction Tokens

**Files:**
- Modify: `ugenix-frontend/src/index.css`

- [ ] **Step 1: Update global styles with CSS variables and core resets**

```css
@tailwind base;
@tailwind components;
@tailwind utilities;

:root {
  --background: 222.2 84% 4.9%;
  --foreground: 210 40% 98%;
  --primary: 262.1 83.3% 57.8%;
  --primary-foreground: 210 40% 98%;
  --surface: 222.2 47.4% 11.2%;
  --accent: 263.4 70% 50.4%;
  --radius-button: 10px;
  --radius-card: 16px;
}

@layer base {
  body {
    @apply bg-background text-slate-50 antialiased selection:bg-violet-500/30;
  }
}

@layer components {
  .focus-ring {
    @apply focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-violet-500 focus-visible:ring-offset-2 focus-visible:ring-offset-background;
  }
  
  .glass-effect {
    @apply bg-surface/80 backdrop-blur-md border border-slate-800;
  }
}
```

- [ ] **Step 2: Commit changes**

```bash
git add ugenix-frontend/src/index.css
git commit -m "style: define global css variables and interaction tokens"
```

---

### Task 3: Build Reusable Button Component

**Files:**
- Create: `ugenix-frontend/src/components/ui/Button.tsx`
- Test: `ugenix-frontend/src/components/ui/__tests__/Button.test.tsx`

- [ ] **Step 1: Write the failing test**

```tsx
import { render, screen } from '@testing-library/react';
import { Button } from '../Button';
import { describe, it, expect } from 'vitest';

describe('Button', () => {
  it('renders correctly with default props', () => {
    render(<Button>Click me</Button>);
    expect(screen.getByRole('button', { name: /click me/i })).toBeInTheDocument();
  });
});
```

- [ ] **Step 2: Implement the Button component**

```tsx
import React from 'react';
import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'ghost';
  size?: 'sm' | 'md' | 'lg';
  isLoading?: boolean;
}

export const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant = 'primary', size = 'md', isLoading, children, ...props }, ref) => {
    const variants = {
      primary: 'bg-amethyst-gradient text-white shadow-[0_4px_14px_0_rgba(109,40,217,0.39)] hover:brightness-110 active:scale-[0.98]',
      secondary: 'bg-surface border border-violet-900/50 text-slate-200 hover:bg-slate-800 active:scale-[0.98]',
      ghost: 'bg-transparent text-violet-400 hover:bg-violet-500/10 active:scale-[0.98]',
    };

    const sizes = {
      sm: 'px-3 py-1.5 text-sm',
      md: 'px-6 py-3 text-base',
      lg: 'px-8 py-4 text-lg',
    };

    return (
      <button
        ref={ref}
        className={cn(
          'relative inline-flex items-center justify-center rounded-button font-semibold transition-all duration-200 focus-ring disabled:opacity-50 disabled:pointer-events-none',
          variants[variant],
          sizes[size],
          className
        )}
        disabled={isLoading || props.disabled}
        {...props}
      >
        {isLoading && (
          <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-current" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
        )}
        {children}
      </button>
    );
  }
);

Button.displayName = 'Button';
```

- [ ] **Step 3: Run tests and verify PASS**

Run: `npm test` (or `vitest run`) in `ugenix-frontend` directory.

- [ ] **Step 4: Commit**

```bash
git add ugenix-frontend/src/components/ui/Button.tsx
git commit -m "feat: add reusable Button component"
```

---

### Task 4: Build Reusable Input Component

**Files:**
- Create: `ugenix-frontend/src/components/ui/Input.tsx`

- [ ] **Step 1: Implement the Input component**

```tsx
import React from 'react';
import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
}

export const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className, label, error, ...props }, ref) => {
    return (
      <div className="w-full space-y-1.5">
        {label && (
          <label className="block text-xs font-medium text-slate-400 uppercase tracking-wider">
            {label}
          </label>
        )}
        <input
          ref={ref}
          className={cn(
            'flex h-12 w-full rounded-lg border border-slate-800 bg-surface px-4 py-2 text-white transition-all duration-200 placeholder:text-slate-500 focus-ring',
            error && 'border-red-500 ring-red-500/20',
            className
          )}
          {...props}
        />
        {error && <p className="text-xs text-red-500 mt-1">{error}</p>}
      </div>
    );
  }
);

Input.displayName = 'Input';
```

- [ ] **Step 2: Commit changes**

```bash
git add ugenix-frontend/src/components/ui/Input.tsx
git commit -m "feat: add reusable Input component"
```

---

### Task 5: Build Reusable Card Component

**Files:**
- Create: `ugenix-frontend/src/components/ui/Card.tsx`

- [ ] **Step 1: Implement the Card component**

```tsx
import React from 'react';
import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const Card = ({ className, children, ...props }: React.HTMLAttributes<HTMLDivElement>) => (
  <div
    className={cn(
      'rounded-card border border-slate-800 bg-surface text-slate-50 shadow-sm transition-all duration-300 hover:border-violet-900/50 hover:-translate-y-1',
      className
    )}
    {...props}
  >
    {children}
  </div>
);

export const CardHeader = ({ className, ...props }: React.HTMLAttributes<HTMLDivElement>) => (
  <div className={cn('flex flex-col space-y-1.5 p-6', className)} {...props} />
);

export const CardContent = ({ className, ...props }: React.HTMLAttributes<HTMLDivElement>) => (
  <div className={cn('p-6 pt-0', className)} {...props} />
);

export const CardFooter = ({ className, ...props }: React.HTMLAttributes<HTMLDivElement>) => (
  <div className={cn('flex items-center p-6 pt-0', className)} {...props} />
);
```

- [ ] **Step 2: Commit changes**

```bash
git add ugenix-frontend/src/components/ui/Card.tsx
git commit -m "feat: add reusable Card components"
```
