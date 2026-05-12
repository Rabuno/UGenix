import api from '../../api/client';

export interface Voucher {
  id: string;
  restaurantId: string;
  code: string;
  price: number;
  discountValue: number;
  remainingStock: number;
  expiresAt: string;
}

export const voucherApi = {
  /**
   * Fetch available vouchers for a restaurant.
   */
  getByRestaurant: async (restaurantId: string): Promise<Voucher[]> => {
    const { data } = await api.get<Voucher[]>(`/v1/voucher/restaurant/${restaurantId}`);
    return data;
  },

  /**
   * Purchase a voucher.
   */
  purchase: async (voucherId: string): Promise<{ orderId: string; status: string }> => {
    const { data } = await api.post<{ orderId: string; status: string }>('/v1/voucher/purchase', { voucherId });
    return data;
  },
};
