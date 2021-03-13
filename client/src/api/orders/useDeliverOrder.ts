import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface DeliverOrderCommand {
  orderId: string;
}

export function useDeliverOrder() {
  const cache = useQueryCache();

  return useMutation<string, ApiError, DeliverOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/deliver`);
      return response.data;
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
