import { create } from 'zustand';

export type ActiveOverlay = 'auth' | 'marketplace' | 'profile' | null;

interface UIState {
  activeOverlay: ActiveOverlay;
  setOverlay: (type: ActiveOverlay) => void;
}

/**
 * Global UI Store for managing the Single Active Overlay state.
 * Source of truth for the overlay type.
 */
export const useUIStore = create<UIState>((set) => ({
  activeOverlay: null,
  setOverlay: (type) => set({ activeOverlay: type }),
}));
