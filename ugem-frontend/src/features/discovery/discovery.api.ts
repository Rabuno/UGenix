import api, { ApiEnvelope } from '../../api/client';

export interface DiscoveryPlace {
  id: string;
  name: string;
  description: string;
  latitude: number;
  longitude: number;
  distanceMeters: number;
  averageRating: number;
  reviewCount: number;
  isTrending: boolean;
}

export interface PagedList<T> {
  items: T[];
  nextCursor: string | null;
  hasMore: boolean;
}

export const discoveryApi = {
  /**
   * Search nearby places using spatial parameters.
   */
  searchNearby: async (lat: number, lng: number, radius: number, cursor?: string): Promise<PagedList<DiscoveryPlace>> => {
    // Note: Interceptor automatically unwraps the ApiEnvelope, returning PagedList<DiscoveryPlace>
    const response = await api.get<PagedList<DiscoveryPlace>>('/v1/discovery/nearby', {
      params: { lat, lng, radius, cursor },
    });
    
    return response as unknown as PagedList<DiscoveryPlace>;
  },
};
