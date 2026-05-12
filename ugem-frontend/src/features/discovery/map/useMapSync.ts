import { useEffect, useRef } from 'react';
import { useDiscoveryStore } from '../discovery.store';
import { GridClusteringStrategy, Cluster } from './cluster';

/**
 * useMapSync: Controlled synchronization hook between Zustand and VietMap GL.
 * Implements render throttling, viewport culling, and batch updates.
 */
export const useMapSync = (mapRef: any, onUpdate: (clusters: Cluster[]) => void) => {
  const { results, center, radius } = useDiscoveryStore();
  const updateTimeoutRef = useRef<NodeJS.Timeout | null>(null);
  const clusterer = useRef(new GridClusteringStrategy());

  useEffect(() => {
    if (!mapRef.current) return;

    const performUpdate = () => {
      if (!mapRef.current) return;

      const zoom = mapRef.current.getZoom();
      const bounds = mapRef.current.getBounds();

      // Viewport-based culling: Filter points inside visible bounds
      const visiblePoints = results.filter(p => {
        return (
          p.longitude >= bounds.getWest() &&
          p.longitude <= bounds.getEast() &&
          p.latitude >= bounds.getSouth() &&
          p.latitude <= bounds.getNorth()
        );
      });

      // Calculate Clusters
      const clusters = clusterer.current.getClusters(visiblePoints, zoom, bounds);
      
      // Batch update the UI
      onUpdate(clusters);
    };

    // Throttled update pipeline (200ms debounce)
    const throttledUpdate = () => {
      if (updateTimeoutRef.current) clearTimeout(updateTimeoutRef.current);
      updateTimeoutRef.current = setTimeout(performUpdate, 200);
    };

    // Listen to map interactions
    mapRef.current.on('moveend', throttledUpdate);
    mapRef.current.on('zoomend', throttledUpdate);

    // Initial and store-triggered updates
    throttledUpdate();

    return () => {
      if (updateTimeoutRef.current) clearTimeout(updateTimeoutRef.current);
      if (mapRef.current) {
        mapRef.current.off('moveend', throttledUpdate);
        mapRef.current.off('zoomend', throttledUpdate);
      }
    };
  }, [results, center, radius, mapRef, onUpdate]);
};
