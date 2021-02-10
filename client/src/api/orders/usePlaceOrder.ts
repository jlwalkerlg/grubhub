import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getActiveOrderQueryKey } from "./useActiveOrder";

export interface PlaceOrderCommand {
  restaurantId: string;
  orderId: string;
  addressLine1: string;
  addressLine2: string;
  addressLine3: string;
  city: string;
  postcode: string;
}

export function usePlaceOrder() {
  const cache = useQueryCache();

  return useMutation<string, ApiError, PlaceOrderCommand, null>(
    async (command) => {
      const { orderId, ...data } = command;
      const response = await Api.post<string>(`/orders/${orderId}/place`, data);
      return response.data;
    },
    {
      onSuccess: (_, { restaurantId }) => {
        cache.invalidateQueries(getActiveOrderQueryKey(restaurantId));
      },
    }
  );
}
