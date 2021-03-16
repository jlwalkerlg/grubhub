import { useQuery, UseQueryOptions } from "react-query";
import Api, { ApiError } from "../api";

export interface OrderModel {
  id: string;
  number: number;
  status: string;
  placedAt: string;
  subtotal: number;
}

export function getRestaurantOrderHistoryQueryKey() {
  return "restaurant-order-history";
}

export default function useRestaurantOrderHistory(
  config?: UseQueryOptions<OrderModel[], ApiError>
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
