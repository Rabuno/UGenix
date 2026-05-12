import React, { useEffect, useRef, useCallback } from 'react';
import { useDiscoveryStore } from '../discovery.store';
import { ENV } from '../../../config/env.config';
import { useMapSync } from '../map/useMapSync';
import { Cluster } from '../map/cluster';

declare global {
  interface Window {
    vietmapgl: any;
  }
}

const MapView: React.FC = () => {
  const mapContainerRef = useRef<HTMLDivElement>(null);
  const mapRef = useRef<any>(null);
  const markersRef = useRef<any[]>([]);
  const { center, radius } = useDiscoveryStore();

  // 1. Initialize Map Singleton
  useEffect(() => {
    if (!mapContainerRef.current || mapRef.current) return;

    const vietmapgl = window.vietmapgl;
    if (!vietmapgl) return;

    mapRef.current = new vietmapgl.Map({
      container: mapContainerRef.current,
      style: `https://maps.vietmap.vn/sdk/styles/vietmap-dark-v1.json?apikey=${ENV.VIETMAP_API_KEY}`,
      center: center ? [center.lng, center.lat] : [106.660172, 10.762622],
      zoom: 13,
    });

    mapRef.current.addControl(new vietmapgl.NavigationControl(), 'top-right');

    return () => {
      if (mapRef.current) {
        mapRef.current.remove();
        mapRef.current = null;
      }
    };
  }, []);

  // 2. Batch Update Render for Clusters/Markers
  const renderMarkers = useCallback((clusters: Cluster[]) => {
    if (!mapRef.current) return;

    // Clear existing markers
    markersRef.current.forEach(m => m.remove());
    markersRef.current = [];

    const vietmapgl = window.vietmapgl;

    clusters.forEach(cluster => {
      const el = document.createElement('div');
      
      if (cluster.count > 1) {
        // Cluster Marker
        el.className = 'w-10 h-10 bg-indigo-600 border-2 border-indigo-400 rounded-full shadow-2xl flex items-center justify-center cursor-pointer transform hover:scale-110 transition-all';
        el.innerHTML = `
          <div class="text-white text-xs font-bold">${cluster.count}</div>
          <div class="absolute -bottom-1 -right-1 w-3 h-3 bg-pink-500 rounded-full border border-white"></div>
        `;
        
        el.onclick = () => {
          mapRef.current.flyTo({
            center: [cluster.longitude, cluster.latitude],
            zoom: mapRef.current.getZoom() + 2
          });
        };
      } else {
        // Individual Marker
        const place = cluster.points[0];
        el.className = 'w-6 h-6 bg-indigo-500 border-2 border-white rounded-full shadow-lg cursor-pointer transform hover:scale-125 transition-transform flex items-center justify-center';
        el.innerHTML = '<div class="w-2 h-2 bg-white rounded-full"></div>';
        
        const popup = new vietmapgl.Popup({ offset: 25 })
          .setHTML(`
            <div class="p-2 min-w-[150px]">
              <h4 class="font-bold text-gray-900">${place.name}</h4>
              <p class="text-[10px] text-gray-600">${(place.distanceMeters / 1000).toFixed(1)} km away</p>
            </div>
          `);

        new vietmapgl.Marker(el)
          .setLngLat([cluster.longitude, cluster.latitude])
          .setPopup(popup)
          .addTo(mapRef.current);
      }

      const marker = new vietmapgl.Marker(el)
        .setLngLat([cluster.longitude, cluster.latitude])
        .addTo(mapRef.current);

      markersRef.current.push(marker);
    });
  }, []);

  // 3. Attach Synchronization Controller
  useMapSync(mapRef, renderMarkers);

  // 4. Sync Radius Circle Overlay (Optimized)
  useEffect(() => {
    if (!mapRef.current || !center) return;

    const sourceId = 'search-radius';
    const vietmapgl = window.vietmapgl;

    // Simplified circle GeoJSON generator
    const generateCircle = (lat: number, lng: number, rad: number) => {
      const points = 64;
      const coords = [];
      const distanceX = rad / 1000 / (111.32 * Math.cos(lat * Math.PI / 180));
      const distanceY = rad / 1000 / 110.574;

      for (let i = 0; i < points; i++) {
        const theta = (i / points) * (2 * Math.PI);
        coords.push([lng + distanceX * Math.cos(theta), lat + distanceY * Math.sin(theta)]);
      }
      coords.push(coords[0]);
      return { type: 'Feature', geometry: { type: 'Polygon', coordinates: [coords] } };
    };

    const circleData = generateCircle(center.lat, center.lng, radius);

    if (mapRef.current.getSource(sourceId)) {
      mapRef.current.getSource(sourceId).setData(circleData);
    } else {
      mapRef.current.addSource(sourceId, { type: 'geojson', data: circleData });
      mapRef.current.addLayer({
        id: `${sourceId}-fill`,
        type: 'fill',
        source: sourceId,
        paint: { 'fill-color': '#6366f1', 'fill-opacity': 0.1 }
      });
    }
  }, [center, radius]);

  return (
    <div className="w-full h-full relative">
      <div ref={mapContainerRef} className="absolute inset-0" />
      <div className="absolute bottom-4 left-4 pointer-events-none">
        <div className="glass-card px-3 py-1.5 text-[10px] font-bold bg-black/40 backdrop-blur-md border-white/5 uppercase tracking-widest text-indigo-400">
          Clustering Active • WebGL Optimized
        </div>
      </div>
    </div>
  );
};

export default MapView;
