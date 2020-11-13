import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

export interface UpdateMenuItemCommand {
  restaurantId: string;
  categoryName: string;
  oldItemName: string;
  newItemName: string;
  description: string;
  price: number;
}

async function updateMenuItem(command: UpdateMenuItemCommand) {
  const { restaurantId, categoryName, oldItemName, ...data } = command;

  await Api.put(
    `/restaurants/${restaurantId}/menu/categories/${categoryName}/items/${oldItemName}`,
    data
  );
}

export default function useUpdateMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, UpdateMenuItemCommand, null>(
    updateMenuItem,
    {
      onSuccess: (_, command) => {
        queryCache.invalidateQueries(getMenuQueryKey(command.restaurantId));
      },
    }
  );
}
