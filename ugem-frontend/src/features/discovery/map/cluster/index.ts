import { DiscoveryPlace } from '../../discovery.api';

export type Cluster = {
  id: string;
  latitude: number;
  longitude: number;
  count: number;
  points: DiscoveryPlace[];
};

/**
 * Clustering Strategy Interface
 */
export interface IClusteringStrategy {
  getClusters(points: DiscoveryPlace[], zoom: number, bounds: unknown): Cluster[];
}

/**
 * Basic Grid-based Clustering Abstraction
 */
export class GridClusteringStrategy implements IClusteringStrategy {
  private cellSize: number = 60; // Grid cell size in pixels

  getClusters(points: DiscoveryPlace[], zoom: number, _bounds: unknown): Cluster[] {
    if (zoom > 16) {
      // Don't cluster at high zoom levels
      return points.map(p => ({
        id: `p-${p.id}`,
        latitude: p.latitude,
        longitude: p.longitude,
        count: 1,
        points: [p]
      }));
    }

    const clusters: Record<string, Cluster> = {};
    
    points.forEach(p => {
      // Very simple grid-based grouping logic
      const latGrid = Math.floor(p.latitude * (zoom + 1) * 10) / 10;
      const lngGrid = Math.floor(p.longitude * (zoom + 1) * 10) / 10;
      const key = `${latGrid}-${lngGrid}`;

      if (!clusters[key]) {
        clusters[key] = {
          id: `c-${key}`,
          latitude: p.latitude,
          longitude: p.longitude,
          count: 0,
          points: []
        };
      }
      
      clusters[key].count++;
      clusters[key].points.push(p);
      
      // Average out the center (simple)
      clusters[key].latitude = (clusters[key].latitude + p.latitude) / 2;
      clusters[key].longitude = (clusters[key].longitude + p.longitude) / 2;
    });

    return Object.values(clusters);
  }
}
