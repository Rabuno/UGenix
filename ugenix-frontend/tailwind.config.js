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
