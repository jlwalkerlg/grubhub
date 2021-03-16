import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getBasketQueryKey } from "./useBasket";

interface RemoveFromBasketCommand {
  restaurantId: string;
  menuItemId: string;
}

export default function useRemoveFromBasket() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, RemoveFromBasketCommand, null>(
    async (command) => {
      await Api.delete(
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
