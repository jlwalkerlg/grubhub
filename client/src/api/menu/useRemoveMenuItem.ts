import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getRestaurantQueryKey } from "../restaurants/useRestaurant";

interface RemoveMenuItemCommand {
  restaurantId: string;
  categoryId: string;
  itemId: string;
}

async function removeMenuItem(command: RemoveMenuItemCommand) {
  const { restaurantId, categoryId, itemId } = command;

  await api.delete(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}/items/${itemId}`
  );
}

export default function useRemoveMenuItem() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, RemoveMenuItemCommand, null>(
    removeMenuItem,
    {
      onSuccess: (_, command) => {
        queryClient.invalidateQueries(
          getRestaurantQueryKey(command.restaurantId)
        );
      },
    }
  );
}
