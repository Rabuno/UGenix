---
# UGenix Design Tokens (Elegant Amethyst)
name: UGenix
version: 1.0.0

tokens:
  colors:
    base:
      background: "#020617" # Slate-950
      surface: "#0f172a"    # Slate-900
    primary:
      gradient:
        from: "#6d28d9"     # Violet-700
        to: "#4c1d95"       # Violet-900
      foreground: "#f8fafc" # Slate-50
    secondary:
      background: "#1e293b" # Slate-800
      foreground: "#e2e8f0" # Slate-200
    accent:
      default: "#8b5cf6"    # Violet-500
      glow: "rgba(139, 92, 246, 0.4)"
    text:
      primary: "#f8fafc"    # Slate-50
      secondary: "#94a3b8"  # Slate-400
  
  typography:
    fontFamily: "Inter, system-ui, -apple-system, sans-serif"
    weights:
      normal: 400
      medium: 500
      semibold: 600
      bold: 700
  
  spacing:
    pagePadding:
      mobile: "16px"
      tablet: "24px"
      desktop: "32px"
    section:
      mobile: "32px"
      desktop: "48px"
    gridGap:
      mobile: "16px"
      desktop: "24px"
  
  borderRadius:
    button: "10px"
    card: "16px"
    input: "8px"
  
  zIndex:
    nav: 40
    sticky: 30
    drawer: 50
    modal: 60
    toast: 70

  elevation:
    card: "subtle-border-low-shadow"
    hover: "violet-border-translate-y"
    overlay: "high-shadow-backdrop-blur"
---

# Design Rationale: UGenix Elegant Amethyst

## Vision
Transform UGenix into a premium spatial discovery marketplace. The UI uses the "Deep Amethyst" design language to project High Performance, Security, and Sophistication.

## Color Strategy
- **Base Background:** Use Slate-950 (`#020617`) for deep space feel.
- **Surface Layering:** Use Slate-900 (`#0f172a`) for cards and elevated sections to create hierarchy without using heavy shadows.
- **Primary Action:** Use the Violet Gradient **strictly** for primary CTAs and active navigation. Avoid overuse to prevent "Neon Overkill".
- **Focus States:** Always use a 2px Violet-500 glow for accessibility and brand alignment.

## Spatial UX Rules
- **Distance Context:** Every restaurant/voucher card MUST show a distance badge (e.g., "1.2 km").
- **Visual Hierarchy:** Use `z-index` layering strictly as defined in tokens to ensure modals, drawers, and toasts never overlap incorrectly.
- **Interactions:** Use a 98% scale-down effect for active button states to provide tactile feedback.

## Accessibility (a11y)
- Target WCAG AA contrast ratios for all text.
- Maintain a clear focus ring for keyboard navigation.
- Use backdrop blur (8px) on overlays to maintain context while focusing on the foreground task.
