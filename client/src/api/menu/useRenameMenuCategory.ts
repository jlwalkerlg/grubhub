import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getMenuQueryKey } from "./useMenu";

export interface RenameMenuCategoryCommand {
  restaurantId: string;
  categoryId: string;
  newName: string;
}

async function renameMenuCategory(command: RenameMenuCategoryCommand) {
  const { restaurantId, categoryId, ...data } = command;

  await Api.put(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}`,
    data
  );
}

export default function useRenameMenuCategory() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, RenameMenuCategoryCommand, null>(
    renameMenuCategory,
    {
      onSuccess: (_, command) => {
        queryCache.invalidateQueries(getMenuQueryKey(command.restaurantId));
      },
    }
  );
}
