import { useQuery } from "react-query";
import api, { ApiError } from "../api";

export interface GetRestaurantOrderHistoryResponse {
  orders: OrderModel[];
  pages: number;
}

export interface OrderModel {
  id: string;
  number: number;
  status: string;
  placedAt: string;
  subtotal: number;
}

export function getRestaurantOrderHistoryQueryKey(page?: number) {
  return page === undefined
    ? "restaurant-order-history"
    : ["restaurant-order-history", page];
}

export default function useRestaurantOrderHistory(page: number) {
  return useQuery<GetRestaurantOrderHistoryResponse, ApiError>(
    getRestaurantOrderHistoryQueryKey(page),
    async () => {
      const { data } = await api.get<GetRestaurantOrderHistoryResponse>(
        "/restaurant/order-history",
        {
          params: {
            page,
          },
        }
      );
      return data;
    },
    {
      keepPreviousData: true,
    }
  );
}
