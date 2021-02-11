import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getBasketQueryKey } from "./useBasket";

interface RemoveFromBasketCommand {
  restaurantId: string;
  menuItemId: string;
}

export default function useRemoveFromBasket() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, RemoveFromBasketCommand, null>(
    async (command) => {
      await Api.delete(
        `/restaurants/${command.restaurantId}/basket/items/${command.menuItemId}`
      );
    },
    {
      onSuccess: (_, command) => {
        cache.invalidateQueries(getBasketQueryKey(command.restaurantId));
      },
    }
  );
}
