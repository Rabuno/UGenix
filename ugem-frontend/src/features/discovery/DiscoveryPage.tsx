import React, { useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import { MapPin, Star, Search, Sliders } from 'lucide-react';
import { discoveryApi, DiscoveryPlace } from './discovery.api';
import { useDiscoveryStore } from './discovery.store';
import MapView from './components/MapView';

export default function DiscoveryPage() {
  const { radius, setRadius, setResults, center, setCenter } = useDiscoveryStore();
  
  // Set default center if not exists
  useEffect(() => {
    if (!center) {
      setCenter(10.762622, 106.660172);
    }
  }, [center, setCenter]);

  const { data, isLoading, error, refetch } = useQuery({
    queryKey: ['discovery', center, radius],
    queryFn: () => discoveryApi.searchNearby(center!.lat, center!.lng, radius),
    enabled: !!center,
  });

  useEffect(() => {
    if (data?.items) {
      setResults(data.items);
    }
  }, [data, setResults]);

  return (
    <div className="h-[calc(100vh-120px)] flex flex-col gap-6 animate-in fade-in duration-700">
      {/* Header Section */}
      <header className="flex flex-col md:flex-row md:items-end justify-between gap-6 shrink-0">
        <div className="space-y-1">
          <h2 className="text-3xl font-bold tracking-tight">Nearby Discovery</h2>
          <p className="text-gray-400 text-sm">Spatial search powered by VietMap Intelligence.</p>
        </div>
        
        <div className="flex items-center gap-4">
          <div className="glass-card flex items-center px-4 py-2 gap-3">
            <Sliders className="w-4 h-4 text-indigo-400" />
            <span className="text-sm font-medium whitespace-nowrap">{radius / 1000}km</span>
            <input 
              type="range" 
              min="1000" 
              max="20000" 
              step="1000"
              value={radius}
              onChange={(e) => setRadius(parseInt(e.target.value))}
              className="w-24 h-1.5 bg-gray-700 rounded-lg appearance-none cursor-pointer accent-indigo-500"
            />
          </div>
          <button onClick={() => refetch()} className="btn-primary flex items-center gap-2">
            <Search className="w-4 h-4" />
            <span>Search</span>
          </button>
        </div>
      </header>

      {/* Split Layout Container */}
      <div className="flex-1 flex flex-col lg:flex-row gap-6 min-h-0">
        {/* Map Section (60%) */}
        <div className="lg:w-3/5 h-[400px] lg:h-full glass-card overflow-hidden relative">
          <MapView />
        </div>

        {/* List Section (40%) */}
        <div className="lg:w-2/5 flex flex-col gap-4 overflow-y-auto pr-2 custom-scrollbar">
          {isLoading ? (
            [...Array(3)].map((_, i) => (
              <div key={i} className="glass-card h-32 shrink-0 animate-pulse bg-white/5" />
            ))
          ) : error ? (
            <div className="glass-card p-12 text-center text-red-400">
              Error loading results.
            </div>
          ) : (
            data?.items.map((place) => (
              <PlaceListCard key={place.id} place={place} />
            ))
          )}
        </div>
      </div>
    </div>
  );
}

function PlaceListCard({ place }: { place: DiscoveryPlace }) {
  return (
    <div className="glass-card p-4 flex gap-4 hover:border-indigo-500/50 transition-all cursor-pointer group">
      <div className="w-24 h-24 bg-indigo-900/20 rounded-xl shrink-0 flex items-center justify-center">
        <MapPin className="w-8 h-8 text-indigo-500/50" />
      </div>
      <div className="flex-1 space-y-2">
        <div className="flex justify-between items-start">
          <h3 className="font-bold text-lg group-hover:text-indigo-400 transition-colors">{place.name}</h3>
          <span className="text-[10px] text-indigo-400 font-bold bg-indigo-400/10 px-2 py-0.5 rounded">
            {(place.distanceMeters / 1000).toFixed(1)}km
          </span>
        </div>
        <div className="flex items-center gap-1">
          <Star className="w-3 h-3 text-yellow-500 fill-yellow-500" />
          <span className="text-sm font-bold">{place.averageRating}</span>
          <span className="text-gray-500 text-[10px]">({place.reviewCount})</span>
        </div>
        <p className="text-gray-400 text-xs line-clamp-1">{place.description}</p>
      </div>
    </div>
  );
}
