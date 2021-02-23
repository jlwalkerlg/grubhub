import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getBasketQueryKey } from "./useBasket";

interface UpdateBasketItemQuantityCommand {
  restaurantId: string;
  menuItemId: string;
  quantity: number;
}

export function useUpdateBasketItemQuantity() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, UpdateBasketItemQuantityCommand, null>(
    async ({ restaurantId, menuItemId, quantity }) => {
      await Api.put(`/restaurants/${restaurantId}/basket/items/${menuItemId}`, {
        quantity,
      });
    },
    {
      onSuccess: (_, command) => {
        cache.invalidateQueries(getBasketQueryKey(command.restaurantId));
      },
    }
  );
}
