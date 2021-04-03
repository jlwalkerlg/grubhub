import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getBasketQueryKey } from "./useBasket";

interface RemoveFromBasketCommand {
  restaurantId: string;
  menuItemId: string;
}

export default function useRemoveFromBasket() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, RemoveFromBasketCommand, null>(
    async (command) => {
      await api.delete(
        `/restaurants/${command.restaurantId}/basket/items/${command.menuItemId}`
      );
    },
    {
      onSuccess: (_, command) => {
        queryClient.invalidateQueries(getBasketQueryKey(command.restaurantId));
      },
    }
  );
}
