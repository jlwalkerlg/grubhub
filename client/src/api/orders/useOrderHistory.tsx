import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "~/api/Api";

export interface OrderModel {
  id: string;
  placedAt: string;
  totalItems: number;
  subtotal: number;
  serviceFee: number;
  deliveryFee: number;
  restaurantName: string;
}

export function getOrderHistoryQueryKey() {
  return "order-history";
}

export function useOrderHistory(config?: QueryConfig<OrderModel[], ApiError>) {
  return useQuery<OrderModel[], ApiError>(
    getOrderHistoryQueryKey(),
    async () => {
      const response = await Api.get<OrderModel[]>("/order-history");
      return response.data;
    },
    config
  );
}
