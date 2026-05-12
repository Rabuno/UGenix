import { create } from 'zustand';
import { DiscoveryPlace } from './discovery.api';

interface DiscoveryState {
  results: DiscoveryPlace[];
  loading: boolean;
  radius: number;
  center: { lat: number; lng: number } | null;
  error: string | null;
  
  setRadius: (radius: number) => void;
  setCenter: (lat: number, lng: number) => void;
  setResults: (results: DiscoveryPlace[]) => void;
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
}

/**
 * Discovery Store managing spatial search state.
 */
export const useDiscoveryStore = create<DiscoveryState>((set) => ({
  results: [],
  loading: false,
  radius: 5000,
  center: null,
  error: null,
  
  setRadius: (radius) => set({ radius }),
  setCenter: (lat, lng) => set({ center: { lat, lng } }),
  setResults: (results) => set({ results, error: null }),
  setLoading: (loading) => set({ loading }),
  setError: (error) => set({ error }),
}));
