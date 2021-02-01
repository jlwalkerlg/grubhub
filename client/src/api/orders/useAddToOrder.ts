import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { activeOrderQueryKey } from "./useActiveOrder";

interface AddToOrderCommand {
  restaurantId: string;
  menuItemId: string;
  quantity: number;
}

export function useAddToOrder() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, AddToOrderCommand, null>(
    async (command) => {
      await Api.post(`/order/${command.restaurantId}`, command);
    },
    {
      onSuccess: (_, command) => {
        cache.invalidateQueries(activeOrderQueryKey(command.restaurantId));
      },
    }
  );
}
