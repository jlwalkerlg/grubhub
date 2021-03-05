import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";

export interface OrderModel {
  id: string;
  number: number;
  status: string;
  placedAt: string;
  deliveredAt: string;
  subtotal: number;
}

function getRestaurantOrderHistoryQueryKey() {
  return "restaurant-order-history";
}

export default function useRestaurantOrderHistory(
  config?: QueryConfig<OrderModel[], ApiError>
) {
  return useQuery<OrderModel[], ApiError>(
    getRestaurantOrderHistoryQueryKey(),
    async () => {
      const response = await Api.get<OrderModel[]>("/restaurant/order-history");
      return response.data;
    },
    config
  );
}
