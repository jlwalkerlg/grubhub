import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface CancelOrderCommand {
  orderId: string;
}

export function useCancelOrder() {
  const queryClient = useQueryClient();

  return useMutation<string, ApiError, CancelOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/cancel`);
      return response.data;
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
