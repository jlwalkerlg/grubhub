import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "~/api/Api";

export interface OrderModel {
  id: string;
  deliveredAt: string;
  totalItems: number;
  subtotal: number;
  serviceFee: number;
  deliveryFee: number;
  restaurantName: string;
}

export function useOrderHistory(config?: QueryConfig<OrderModel[], ApiError>) {
  return useQuery<OrderModel[], ApiError>(
    "order-history",
    async () => {
      const response = await Api.get<OrderModel[]>("/order-history");
      return response.data;
    },
    config
  );
}
