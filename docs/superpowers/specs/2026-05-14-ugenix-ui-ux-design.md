# Design Document: UGenix UI/UX Reconstruction (Elegant Amethyst) - v2.1

**Date:** 2026-05-14
**Status:** Finalized (Production-Ready)
**Topic:** Design System, UX Architecture & Spatial Experience

## 1. Vision & Goals
Transform UGenix into a premium spatial discovery marketplace using a "Deep Amethyst" design language. The UI must reflect the project's core values: High Performance, Security, and Sophistication.

## 2. Design System: Elegant Amethyst
### 2.1 Color Palette & Usage Rules
- **Base:** Slate-950 (`#020617`) - Main background.
- **Surface:** Slate-900 (`#0f172a`) - Cards, elevated sections, and sidebar.
- **Primary Gradient:** Violet-700 (`#6d28d9`) to Violet-900 (`#4c1d95`).
- **Accent:** Violet-500 (`#8b5cf6`) - Interactive states and focus rings.
- **Text:** Slate-50 (Primary), Slate-400 (Secondary).

**Usage Rules:**
- Use **Violet Gradient** ONLY for Primary CTAs, active navigation items, and high-importance badges.
- Use **Slate-900** for cards to prevent "Neon Overkill".
- Focus rings: 2px Violet-500 glow.

### 2.2 Typography & Spacing
- **Font:** Inter / System-UI.
- **Spacing System:**
  - Page Padding: 16px (Mobile), 24px (Tablet), 32px (Desktop).
  - Section Spacing: 32px (Mobile), 48px (Desktop).
  - Grid Gap: 16px (Mobile), 24px (Desktop).

### 2.3 Elevation & Z-Index
- **Elevation:**
  - Card: Subtle border + low shadow.
  - Hover Card: Stronger violet/slate border + translateY(-2px).
  - Modal/Drawer: High shadow + backdrop blur (8px).
- **Z-Index Layering:**
  - Header/Navbar: `z-40`
  - Sticky CTA/Sidebar: `z-30`
  - Drawer/Overlays: `z-50`
  - Modal: `z-60`
  - Toast: `z-70`

## 3. Page-Specific Layouts
### 3.1 Home / Discovery
- **Hero:** Search section with location-aware radius detection.
- **Nearby:** "Restaurants Near You" grid with distance badges.
- **Featured:** Top voucher deals with large visual cards.
- **Categories:** Horizontal pill navigation for quick filtering.

### 3.2 Restaurant Detail
- **Hero:** Image gallery with overlay info.
- **Layout:** 2-column (Desktop). Left: Overview/Reviews. Right (Sticky): Location & CTA.
- **Voucher Section:** Grid of offers specific to the restaurant.

### 3.3 Voucher Marketplace
- **Filter Sidebar:** Desktop persistent / Mobile drawer.
- **Grid:** 3-column (Desktop), 2-column (Tablet), 1-column (Mobile).

## 4. Spatial UX Rules
- **Distance Badges:** Always show "X.X km" on restaurant/voucher cards.
- **Location Status:** If GPS is denied, show prominent manual location selector.
- **Radius Filter:** Presets for 1km, 3km, 5km, 10km.

## 5. Interaction States & a11y
- **States:** Default, Hover, Active (98% scale), Focus, Loading (Skeleton), Empty, Error.
- **Accessibility:** WCAG AA Contrast, Tab-focus support, Modal focus-trap, 'Escape' to close.

## 6. Implementation Roadmap
### Phase 1: Foundation (Design System)
- Update `tailwind.config.js` with Amethyst tokens.
- Build reusable UI Library in `src/components/ui`.

### Phase 2: Feature UI
- Implement Home, Detail, Marketplace, and Profile layouts.

### Phase 3: UX Hardening
- Framer Motion transitions, Skeletons, and Toasts.
