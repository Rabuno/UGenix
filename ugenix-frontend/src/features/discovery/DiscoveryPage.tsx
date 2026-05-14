import React, { useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import { MapPin, Star, Search, Sliders } from 'lucide-react';
import { motion } from 'framer-motion';
import { discoveryApi, DiscoveryPlace } from './discovery.api';
import { useDiscoveryStore } from './discovery.store';
import MapView from './components/MapView';
import { Button } from '../../components/ui/Button';
import { Card, CardContent } from '../../components/ui/Card';
import { Skeleton } from '../../components/ui/Skeleton';

export default function DiscoveryPage() {
  const { radius, setRadius, setResults, center, setCenter } = useDiscoveryStore();
  
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
    <motion.div 
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      className="h-[calc(100vh-120px)] flex flex-col gap-6"
    >
      {/* Header Section */}
      <header className="flex flex-col md:flex-row md:items-end justify-between gap-6 shrink-0">
        <div className="space-y-2">
          <h2 className="text-3xl font-bold tracking-tight text-slate-50">Nearby Discovery</h2>
          <p className="text-slate-400 text-sm">Spatial search powered by VietMap Intelligence.</p>
        </div>
        
        <div className="flex items-center gap-4">
          <Card className="flex items-center px-4 py-2 gap-3 h-12 shadow-none">
            <Sliders className="w-4 h-4 text-violet-400" />
            <span className="text-sm font-medium whitespace-nowrap text-slate-200">{radius / 1000}km</span>
            <input 
              type="range" 
              min="1000" 
              max="20000" 
              step="1000"
              value={radius}
              onChange={(e) => setRadius(parseInt(e.target.value))}
              className="w-24 h-1.5 bg-slate-800 rounded-lg appearance-none cursor-pointer accent-violet-500"
            />
          </Card>
          <Button onClick={() => refetch()} className="flex items-center gap-2 h-12">
            <Search className="w-4 h-4" />
            <span>Search</span>
          </Button>
        </div>
      </header>

      {/* Split Layout Container */}
      <div className="flex-1 flex flex-col lg:flex-row gap-6 min-h-0">
        {/* Map Section (60%) */}
        <Card className="lg:w-3/5 h-[400px] lg:h-full overflow-hidden relative shadow-md">
          <MapView />
        </Card>

        {/* List Section (40%) */}
        <div className="lg:w-2/5 flex flex-col gap-4 overflow-y-auto pr-2 custom-scrollbar">
          {isLoading ? (
            [...Array(4)].map((_, i) => (
              <Card key={i} className="shadow-none border-slate-800/50">
                <CardContent className="p-4 flex gap-4">
                  <Skeleton className="w-24 h-24 rounded-xl shrink-0" />
                  <div className="flex-1 space-y-3 py-1">
                    <Skeleton className="h-5 w-3/4" />
                    <Skeleton className="h-4 w-1/4" />
                    <Skeleton className="h-3 w-full mt-4" />
                    <Skeleton className="h-3 w-5/6" />
                  </div>
                </CardContent>
              </Card>
            ))
          ) : error ? (
            <Card className="border-red-500/20 bg-red-500/5">
              <CardContent className="p-12 text-center text-red-400 font-medium">
                Error loading results. Please try again.
              </CardContent>
            </Card>
          ) : (
            data?.items.map((place, i) => (
              <motion.div
                initial={{ opacity: 0, x: 20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ delay: i * 0.05 }}
                key={place.id}
              >
                <PlaceListCard place={place} />
              </motion.div>
            ))
          )}
        </div>
      </div>
    </motion.div>
  );
}

function PlaceListCard({ place }: { place: DiscoveryPlace }) {
  return (
    <Card className="group cursor-pointer hover:border-violet-500/50 shadow-none">
      <CardContent className="p-4 flex gap-4">
        <div className="w-24 h-24 bg-surface rounded-xl shrink-0 flex items-center justify-center border border-slate-800 group-hover:bg-violet-900/20 group-hover:border-violet-500/30 transition-all">
          <MapPin className="w-8 h-8 text-violet-500/50 group-hover:text-violet-400 transition-colors" />
        </div>
        <div className="flex-1 space-y-2">
          <div className="flex justify-between items-start">
            <h3 className="font-bold text-lg text-slate-50 group-hover:text-violet-400 transition-colors line-clamp-1">{place.name}</h3>
            <span className="text-[10px] text-violet-300 font-bold bg-violet-500/20 px-2 py-1 rounded-md shrink-0 ml-2">
              {(place.distanceMeters / 1000).toFixed(1)}km
            </span>
          </div>
          <div className="flex items-center gap-1.5">
            <Star className="w-3.5 h-3.5 text-amber-500 fill-amber-500" />
            <span className="text-sm font-bold text-slate-200">{place.averageRating}</span>
            <span className="text-slate-500 text-xs">({place.reviewCount} reviews)</span>
          </div>
          <p className="text-slate-400 text-xs line-clamp-2 leading-relaxed">{place.description}</p>
        </div>
      </CardContent>
    </Card>
  );
}
