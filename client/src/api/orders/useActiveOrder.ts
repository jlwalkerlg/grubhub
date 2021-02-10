import { QueryConfig, useQuery, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import useAuth from "../users/useAuth";
import { OrderDto } from "./OrderDto";
import { getOrderQueryKey } from "./useOrder";

export function getActiveOrderQueryKey(restaurantId: string) {
  return `/restaurants/${restaurantId}/order`;
}

export default function useActiveOrder(
  restaurantId: string,
  config: QueryConfig<OrderDto, ApiError> = {}
) {
  const { isLoggedIn } = useAuth();

  const cache = useQueryCache();

  return useQuery<OrderDto, ApiError>(
    getActiveOrderQueryKey(restaurantId),
    async () => {
      const response = await Api.get<OrderDto>(
        `/restaurants/${restaurantId}/order`
      );
      return response.data;
    },
    {
      ...config,
      enabled: isLoggedIn && (config.enabled ?? true),
      onSuccess: (order) => {
        cache.setQueryData(getOrderQueryKey(order.id), order);

        if (config.onSuccess) {
          config.onSuccess(order);
        }
      },
    }
  );
}
