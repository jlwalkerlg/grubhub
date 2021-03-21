import { useInfiniteQuery, UseInfiniteQueryOptions } from "react-query";
import api, { ApiError } from "../api";

export interface GetRestaurantOrderHistoryResponse {
  orders: OrderModel[];
  count: number;
}

export interface OrderModel {
  id: string;
  number: number;
  status: string;
  placedAt: string;
  subtotal: number;
}

interface QueryParams {
  perPage: number;
}

export function getRestaurantOrderHistoryQueryKey() {
  return "restaurant-order-history";
}

export default function useRestaurantOrderHistory(
  { perPage }: QueryParams,
  config?: UseInfiniteQueryOptions<GetRestaurantOrderHistoryResponse, ApiError>
) {
  return useInfiniteQuery<GetRestaurantOrderHistoryResponse, ApiError>(
    getRestaurantOrderHistoryQueryKey(),
    async ({ pageParam: page }) => {
      const response = await api.get<GetRestaurantOrderHistoryResponse>(
        "/restaurant/order-history",
        {
          params: {
            page: page ?? 1,
            perPage,
          },
        }
      );
      return response.data;
    },
    config
  );
}
