import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getRestaurantOrderHistoryQueryKey } from "./useRestaurantOrderHistory";

interface RejectOrderCommand {
  orderId: string;
}

export function useRejectOrder() {
  const queryClient = useQueryClient();

  return useMutation<string, ApiError, RejectOrderCommand, null>(
    async ({ orderId }) => {
      const response = await api.put(`/orders/${orderId}/reject`);
      return response.data;
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getRestaurantOrderHistoryQueryKey());
      },
    }
  );
}
