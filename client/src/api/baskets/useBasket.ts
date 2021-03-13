import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../api";
import useAuth from "../users/useAuth";

export interface BasketDto {
  userId: string;
  restaurantId: string;
  items: BasketItemDto[];
}

export interface BasketItemDto {
  menuItemId: string;
  menuItemName: string;
  menuItemDescription: string;
  menuItemPrice: number;
  quantity: number;
}

export function getBasketQueryKey(restaurantId: string) {
  return `/restaurants/${restaurantId}/basket`;
}

export default function useBasket(
  restaurantId: string,
  config: QueryConfig<BasketDto, ApiError> = {}
) {
  const { isLoggedIn } = useAuth();

  return useQuery<BasketDto, ApiError>(
    getBasketQueryKey(restaurantId),
    async () => {
      const response = await Api.get<BasketDto>(
        `/restaurants/${restaurantId}/basket`
      );
      return response.data;
    },
    {
      ...config,
      enabled: isLoggedIn && (config.enabled ?? true),
    }
  );
}
