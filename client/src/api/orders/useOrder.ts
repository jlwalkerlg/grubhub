import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";
import { OrderDto } from "./OrderDto";

export function getOrderQueryKey(orderId: string) {
  return `/orders/${orderId}`;
}

export default function useOrder(
  orderId: string,
  config?: QueryConfig<OrderDto, ApiError>
) {
  return useQuery<OrderDto, ApiError>(
    getOrderQueryKey(orderId),
    async () => {
      const response = await Api.get<OrderDto>(`/orders/${orderId}`);
      return response.data;
    },
    config
  );
}
