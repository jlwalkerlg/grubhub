import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import { getRestaurantQueryKey } from "../restaurants/useRestaurant";

interface RemoveMenuItemCommand {
  restaurantId: string;
  categoryId: string;
  itemId: string;
}

async function removeMenuItem(command: RemoveMenuItemCommand) {
  const { restaurantId, categoryId, itemId } = command;

  await Api.delete(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}/items/${itemId}`
  );
}

export default function useRemoveMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, RemoveMenuItemCommand, null>(
    removeMenuItem,
    {
      onSuccess: (_, command) => {
        queryCache.invalidateQueries(
          getRestaurantQueryKey(command.restaurantId)
        );
      },
    }
  );
}
