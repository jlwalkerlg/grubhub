import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface CancelOrderCommand {
  orderId: string;
}

export function useCancelOrder() {
  const queryClient = useQueryClient();

  return useMutation<string, ApiError, CancelOrderCommand, null>(
    async ({ orderId }) => {
      const response = await api.put(`/orders/${orderId}/cancel`);
      return response.data;
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
