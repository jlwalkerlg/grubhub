import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { activeOrderQueryKey } from "./useActiveOrder";

interface RemoveFromOrderCommand {
  menuItemId: string;
}

export default function useRemoveFromOrder() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, RemoveFromOrderCommand, null>(
    async (command) => {
      await Api.delete(`/order/items/${command.menuItemId}`);
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(activeOrderQueryKey());
      },
    }
  );
}
