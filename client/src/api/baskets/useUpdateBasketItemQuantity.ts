import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getBasketQueryKey } from "./useBasket";

interface UpdateBasketItemQuantityCommand {
  restaurantId: string;
  menuItemId: string;
  quantity: number;
}

export function useUpdateBasketItemQuantity() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, UpdateBasketItemQuantityCommand, null>(
    async ({ restaurantId, menuItemId, quantity }) => {
      await api.put(`/restaurants/${restaurantId}/basket/items/${menuItemId}`, {
        quantity,
      });
    },
    {
      onSuccess: (_, command) => {
        queryClient.invalidateQueries(getBasketQueryKey(command.restaurantId));
      },
    }
  );
}
