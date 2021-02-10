import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getActiveOrderQueryKey } from "./useActiveOrder";

interface RemoveFromOrderCommand {
  restaurantId: string;
  menuItemId: string;
}

export default function useRemoveFromOrder() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, RemoveFromOrderCommand, null>(
    async (command) => {
      await Api.delete(
        `/order/${command.restaurantId}/items/${command.menuItemId}`
      );
    },
    {
      onSuccess: (_, command) => {
        cache.invalidateQueries(getActiveOrderQueryKey(command.restaurantId));
      },
    }
  );
}
