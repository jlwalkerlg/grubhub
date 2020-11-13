import { useQuery } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { MenuDto } from "./MenuDto";

export function getMenuQueryKey(restaurantId: string) {
  return ["menus", restaurantId];
}

async function getMenu(restaurantId: string) {
  const response = await Api.get<MenuDto>(`/restaurants/${restaurantId}/menu`);

  if (!response.isSuccess) {
    throw response.error;
  }

  return response.data;
}

export default function useMenu(restaurantId: string) {
  return useQuery<MenuDto, Error>(getMenuQueryKey(restaurantId), async () =>
    getMenu(restaurantId)
  );
}
