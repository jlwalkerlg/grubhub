import { QueryConfig, useQuery, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import useAuth from "../users/useAuth";
import { OrderDto } from "./OrderDto";

export function getOrderQueryKey(orderId: string) {
  return `/orders/${orderId}`;
}

export default function useOrder(
  orderId: string,
  config: QueryConfig<OrderDto, ApiError> = {}
) {
  const { isLoggedIn } = useAuth();

  const cache = useQueryCache();

  return useQuery<OrderDto, ApiError>(
    getOrderQueryKey(orderId),
    async () => {
      const response = await Api.get<OrderDto>(`/orders/${orderId}`);
      return response.data;
    },
    {
      ...config,
      enabled: isLoggedIn && (config.enabled ?? true),
    }
  );
}
