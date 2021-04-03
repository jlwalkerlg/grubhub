import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../api";
import { getOrderHistoryQueryKey } from "./useOrderHistory";

export interface PlaceOrderCommand {
  mobile: string;
  restaurantId: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  postcode: string;
}

export function usePlaceOrder() {
  const queryClient = useQueryClient();

  return useMutation<string, ApiError, PlaceOrderCommand, null>(
    async (command) => {
      const { restaurantId, ...data } = command;
      const response = await api.post<string>(
        `/restaurants/${restaurantId}/orders`,
        data
      );
      return response.data;
    },
    {
      onSuccess: (_, { restaurantId }) => {
        queryClient.invalidateQueries(getOrderHistoryQueryKey());
      },
    }
  );
}
