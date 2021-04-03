import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getBasketQueryKey } from "./useBasket";

interface AddToBasketCommand {
  restaurantId: string;
  menuItemId: string;
  quantity: number;
}

export function useAddToBasket() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, AddToBasketCommand, null>(
    async (command) => {
      await api.post(`/restaurants/${command.restaurantId}/basket`, command);
    },
    {
      onSuccess: (_, command) => {
        queryClient.invalidateQueries(getBasketQueryKey(command.restaurantId));
      },
    }
  );
}
