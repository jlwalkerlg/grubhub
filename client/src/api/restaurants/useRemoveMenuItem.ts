import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

async function removeMenuItem(
  restaurantId: string,
  category: string,
  item: string
) {
  const response = await Api.delete(
    `/restaurants/${restaurantId}/menu/categories/${category}/items/${item}`
  );

  if (!response.isSuccess) {
    throw response.error;
  }
}

interface Variables {
  restaurantId: string;
  category: string;
  item: string;
}

export default function useRemoveMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, Variables, null>(
    async ({ restaurantId, category, item }) => {
      await removeMenuItem(restaurantId, category, item);
    },
    {
      onSuccess: (_, { restaurantId }) => {
        queryCache.invalidateQueries(getMenuQueryKey(restaurantId));
      },
    }
  );
}
