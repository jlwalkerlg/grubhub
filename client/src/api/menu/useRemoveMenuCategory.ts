import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getMenuQueryKey } from "./useMenu";

interface RemoveMenuCategoryCommand {
  restaurantId: string;
  categoryId: string;
}

async function removeMenuCategory(command: RemoveMenuCategoryCommand) {
  const { restaurantId, categoryId } = command;

  await Api.delete(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}`
  );
}

export default function useRemoveMenuCategory() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, RemoveMenuCategoryCommand, null>(
    removeMenuCategory,
    {
      onSuccess: (_, command) => {
        queryCache.invalidateQueries(getMenuQueryKey(command.restaurantId));
      },
    }
  );
}
