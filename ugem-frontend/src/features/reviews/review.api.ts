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
    const { data } = await api.post<string>('/v1/reviews', request);
    return data;
  },
};
