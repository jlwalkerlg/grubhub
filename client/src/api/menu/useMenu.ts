import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";
import { MenuDto } from "./MenuDto";

export function getMenuQueryKey(restaurantId: string) {
  return ["menus", restaurantId];
}

async function getMenu(restaurantId: string) {
  const response = await Api.get<MenuDto>(`/restaurants/${restaurantId}/menu`);
  return response.data;
}

export default function useMenu(
  restaurantId: string,
  config?: QueryConfig<MenuDto, ApiError>
) {
  return useQuery<MenuDto, ApiError>(
    getMenuQueryKey(restaurantId),
    () => getMenu(restaurantId),
    config
  );
}
