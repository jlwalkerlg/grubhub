import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface DeliverOrderCommand {
  orderId: string;
}

export function useDeliverOrder() {
  const queryClient = useQueryClient();

  return useMutation<string, ApiError, DeliverOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/deliver`);
      return response.data;
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
