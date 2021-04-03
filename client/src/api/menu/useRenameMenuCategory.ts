import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getRestaurantQueryKey } from "../restaurants/useRestaurant";

export interface RenameMenuCategoryCommand {
  restaurantId: string;
  categoryId: string;
  newName: string;
}

async function renameMenuCategory(command: RenameMenuCategoryCommand) {
  const { restaurantId, categoryId, ...data } = command;

  await api.put(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}`,
    data
  );
}

export default function useRenameMenuCategory() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, RenameMenuCategoryCommand, null>(
    renameMenuCategory,
    {
      onSuccess: (_, command) => {
        queryClient.invalidateQueries(
          getRestaurantQueryKey(command.restaurantId)
        );
      },
    }
  );
}
