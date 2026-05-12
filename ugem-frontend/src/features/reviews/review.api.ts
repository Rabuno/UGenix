import api from '../../api/client';

export interface SubmitReviewRequest {
  restaurantId: string;
  rating: number;
  comment: string;
}

export const reviewApi = {
  /**
   * Submit a new review for a restaurant.
   */
  submit: async (request: SubmitReviewRequest): Promise<string> => {
    return await api.post<string>('/v1/review', request) as unknown as string;
  },
};
