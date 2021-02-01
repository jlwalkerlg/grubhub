import { useQuery } from "react-query";
import Api, { ApiError } from "../Api";
import useAuth from "../users/useAuth";
import { OrderDto } from "./OrderDto";

export function activeOrderQueryKey(restaurantId: string) {
  return `/order/${restaurantId}`;
}

export default function useActiveOrder(restaurantId: string) {
  const { isLoggedIn } = useAuth();

  return useQuery<OrderDto, ApiError>(
    activeOrderQueryKey(restaurantId),
    async () => {
      const response = await Api.get<OrderDto>(`/order/${restaurantId}`);
      return response.data;
    },
    {
      enabled: isLoggedIn,
    }
  );
}
