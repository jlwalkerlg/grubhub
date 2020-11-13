import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

export interface UpdateMenuItemRequest {
  newItemName: string;
  description: string;
  price: number;
}

async function updateMenuItem(
  restaurantId: string,
  category: string,
  item: string,
  request: UpdateMenuItemRequest
) {
  const response = await Api.put(
    `/restaurants/${restaurantId}/menu/categories/${category}/items/${item}`,
    request
  );

  if (!response.isSuccess) {
    throw response.error;
  }
}

interface Variables {
  restaurantId: string;
  category: string;
  item: string;
  request: UpdateMenuItemRequest;
}

export default function useUpdateMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, Variables, null>(
    async ({ restaurantId, category, item, request }) => {
      await updateMenuItem(restaurantId, category, item, request);
    },
    {
      onSuccess: (_, { restaurantId }) => {
        queryCache.invalidateQueries(getMenuQueryKey(restaurantId));
      },
    }
  );
}
