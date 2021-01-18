import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getRestaurantQueryKey } from "../restaurants/useRestaurant";

export interface AddMenuItemCommand {
  restaurantId: string;
  categoryId: string;
  name: string;
  description: string;
  price: number;
}

async function addMenuItem(command: AddMenuItemCommand) {
  const { restaurantId, categoryId, ...data } = command;

  await Api.post(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}/items`,
    data
  );
}

export default function useAddMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, AddMenuItemCommand, null>(addMenuItem, {
    onSuccess: (_, command) => {
      queryCache.invalidateQueries(getRestaurantQueryKey(command.restaurantId));
    },
  });
}
