import { useInfiniteQuery, UseInfiniteQueryOptions } from "react-query";
import Api, { ApiError } from "~/api/api";

export interface GetOrderHistoryResponse {
  orders: OrderModel[];
  count: number;
}

export interface OrderModel {
  id: string;
  placedAt: string;
  totalItems: number;
  subtotal: number;
  serviceFee: number;
  deliveryFee: number;
  restaurantName: string;
}

interface QueryParams {
  perPage: number;
}

export function getOrderHistoryQueryKey() {
  return "order-history";
}

export function useOrderHistory(
  { perPage }: QueryParams,
  config?: UseInfiniteQueryOptions<GetOrderHistoryResponse, ApiError>
) {
  return useInfiniteQuery<GetOrderHistoryResponse, ApiError>(
    getOrderHistoryQueryKey(),
    async ({ pageParam: page }) => {
      const response = await Api.get<GetOrderHistoryResponse>(
        "/order-history",
        {
          params: {
            page,
            perPage: perPage,
          },
        }
      );
      return response.data;
    },
    {
      getNextPageParam: (_, pages) => {
        const totalOrdersAvailable = pages[0].count;

        const numberOfOrdersLoaded = pages.reduce(
          (count, page) => count + page.orders.length,
          0
        );

        if (numberOfOrdersLoaded === totalOrdersAvailable) return undefined;

        return pages.length + 1;
      },
      ...config,
    }
  );
}
