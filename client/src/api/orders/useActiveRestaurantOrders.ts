import { useQuery, UseQueryOptions } from "react-query";
import api, { ApiError } from "../api";
import { OrderStatus } from "./useOrder";

export interface ActiveOrderDto {
  id: string;
  number: number;
  subtotal: number;
  status: OrderStatus;
  addressLine1: string;
  addressLine2: string;
  city: string;
  postcode: string;
  placedAt: string;
  estimatedDeliveryTime: string;
}

interface QueryParams {
  confirmedAfter?: Date | string;
}

function getActiveRestaurantOrdersQueryKey() {
  return "active-restaurant-orders";
}

export default function useActiveRestaurantOrders(
  params?: QueryParams,
  config?: UseQueryOptions<ActiveOrderDto[], ApiError>
) {
  return useQuery<ActiveOrderDto[], ApiError>(
    getActiveRestaurantOrdersQueryKey(),
    async () => {
      const response = await api.get<ActiveOrderDto[]>(
        "/restaurant/active-orders",
        {
          params,
        }
      );
      return response.data;
    },
    config
  );
}
