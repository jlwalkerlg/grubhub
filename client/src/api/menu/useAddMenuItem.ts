import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getRestaurantQueryKey } from "../restaurants/useRestaurant";

export interface AddMenuItemCommand {
  restaurantId: string;
  categoryId: string;
  name: string;
  description: string;
  price: number;
}

async function addMenuItem(command: AddMenuItemCommand) {
  const { restaurantId, categoryId, ...data } = command;

  await api.post(
    `/restaurants/${restaurantId}/menu/categories/${categoryId}/items`,
    data
  );
}

export default function useAddMenuItem() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, AddMenuItemCommand, null>(addMenuItem, {
    onSuccess: (_, command) => {
      queryClient.invalidateQueries(
        getRestaurantQueryKey(command.restaurantId)
      );
    },
  });
}
