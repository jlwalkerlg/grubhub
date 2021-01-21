import { useQuery } from "react-query";
import Api, { ApiError } from "../Api";
import useAuth from "../users/useAuth";
import { OrderDto } from "./OrderDto";

export function activeOrderQueryKey() {
  return "/order";
}

export default function useActiveOrder() {
  const { isLoggedIn } = useAuth();

  return useQuery<OrderDto, ApiError>(
    activeOrderQueryKey(),
    async () => {
      const response = await Api.get<OrderDto>("/order");
      return response.data;
    },
    {
      enabled: isLoggedIn,
    }
  );
}
