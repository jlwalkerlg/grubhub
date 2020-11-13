import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

export interface AddMenuItemCommand {
  restaurantId: string;
  categoryName: string;
  itemName: string;
  description: string;
  price: number;
}

async function addMenuItem(command: AddMenuItemCommand) {
  const { restaurantId, ...data } = command;

  await Api.post(`/restaurants/${restaurantId}/menu/items`, data);
}

export default function useAddMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, AddMenuItemCommand, null>(addMenuItem, {
    onSuccess: (_, command) => {
      queryCache.invalidateQueries(getMenuQueryKey(command.restaurantId));
    },
  });
}
