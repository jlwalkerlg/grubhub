import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../api";
import { getRestaurantQueryKey } from "../restaurants/useRestaurant";

export interface AddMenuCategoryCommand {
  restaurantId: string;
  name: string;
}

async function addMenuCategory(command: AddMenuCategoryCommand) {
  const { restaurantId, ...data } = command;

  await api.post(`/restaurants/${restaurantId}/menu/categories`, data);
}

export default function useAddMenuCategory() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, AddMenuCategoryCommand, null>(
    addMenuCategory,
    {
      onSuccess: (_, command) => {
        queryClient.invalidateQueries(
          getRestaurantQueryKey(command.restaurantId)
        );
      },
    }
  );
}
