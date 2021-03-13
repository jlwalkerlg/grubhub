import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface CancelOrderCommand {
  orderId: string;
}

export function useCancelOrder() {
  const cache = useQueryCache();

  return useMutation<string, ApiError, CancelOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/cancel`);
      return response.data;
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
