import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

interface RemoveMenuItemCommand {
  restaurantId: string;
  categoryName: string;
  itemName: string;
}

async function removeMenuItem(command: RemoveMenuItemCommand) {
  const { restaurantId, categoryName, itemName } = command;

  await Api.delete(
    `/restaurants/${restaurantId}/menu/categories/${categoryName}/items/${itemName}`
  );
}

export default function useRemoveMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, RemoveMenuItemCommand, null>(
    removeMenuItem,
    {
      onSuccess: (_, command) => {
        queryCache.invalidateQueries(getMenuQueryKey(command.restaurantId));
      },
    }
  );
}
