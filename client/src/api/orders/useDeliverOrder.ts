import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface DeliverOrderCommand {
  orderId: string;
}

export function useDeliverOrder() {
  const queryClient = useQueryClient();

  return useMutation<string, ApiError, DeliverOrderCommand, null>(
    async ({ orderId }) => {
      const response = await api.put(`/orders/${orderId}/deliver`);
      return response.data;
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
