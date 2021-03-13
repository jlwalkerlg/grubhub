import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface RejectOrderCommand {
  orderId: string;
}

export function useRejectOrder() {
  const cache = useQueryCache();

  return useMutation<string, ApiError, RejectOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/reject`);
      return response.data;
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
