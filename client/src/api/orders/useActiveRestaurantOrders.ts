import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";
import { OrderDto } from "./OrderDto";

interface QueryParams {
  confirmedAfter?: Date | string;
}

function getActiveRestaurantOrdersQueryKey() {
  return "active-restaurant-orders";
}

export default function useActiveRestaurantOrders(
  params?: QueryParams,
  config?: QueryConfig<OrderDto[], ApiError>
) {
  return useQuery<OrderDto[], ApiError>(
    getActiveRestaurantOrdersQueryKey(),
    async () => {
      const response = await Api.get<OrderDto[]>("/restaurant/active-orders", {
        params,
      });
      return response.data;
    },
    config
  );
}
