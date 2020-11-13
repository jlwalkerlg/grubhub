import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

interface RemoveMenuCategoryCommand {
  restaurantId: string;
  categoryName: string;
}

async function removeMenuCategory(command: RemoveMenuCategoryCommand) {
  const { restaurantId, categoryName } = command;

  await Api.delete(
    `/restaurants/${restaurantId}/menu/categories/${categoryName}`
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
