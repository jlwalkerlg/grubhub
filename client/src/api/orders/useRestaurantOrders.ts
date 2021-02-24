import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";
import { OrderDto } from "./OrderDto";

export function getRestaurantOrdersQueryKey() {
  return "/restaurant/orders";
}

export default function useRestaurantOrders(
  config?: QueryConfig<OrderDto[], ApiError>
) {
  return useQuery<OrderDto[], ApiError>(
    getRestaurantOrdersQueryKey(),
    async () => {
      const response = await Api.get<OrderDto[]>("/restaurant/orders");
      return response.data;
    },
    config
  );
}
