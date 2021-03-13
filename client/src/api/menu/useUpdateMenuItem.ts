import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import { getRestaurantQueryKey } from "../restaurants/useRestaurant";

export interface UpdateMenuItemCommand {
  restaurantId: string;
  categoryId: string;
  itemId: string;
  name: string;
  description: string;
  price: number;
}

async function updateMenuItem(command: UpdateMenuItemCommand) {
  const { restaurantId, categoryId, itemId, ...data } = command;

  await Api.put(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}/items/${itemId}`,
    data
  );
}

export default function useUpdateMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, UpdateMenuItemCommand, null>(
    updateMenuItem,
    {
      onSuccess: (_, command) => {
        queryCache.invalidateQueries(
          getRestaurantQueryKey(command.restaurantId)
        );
      },
    }
  );
}
