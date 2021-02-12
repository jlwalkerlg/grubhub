import { useMutation } from "react-query";
import Api, { ApiError } from "../Api";

export interface PlaceOrderCommand {
  mobile: string;
  restaurantId: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  postcode: string;
}

export function usePlaceOrder() {
  return useMutation<string, ApiError, PlaceOrderCommand, null>(
    async (command) => {
      const { restaurantId, ...data } = command;
      const response = await Api.post<string>(
        `/restaurants/${restaurantId}/orders`,
        data
      );
      return response.data;
    }
  );
}
