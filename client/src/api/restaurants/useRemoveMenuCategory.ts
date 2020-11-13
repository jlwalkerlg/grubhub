import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

async function removeMenuCategory(restaurantId: string, category: string) {
  const response = await Api.delete(
    `/restaurants/${restaurantId}/menu/categories/${category}`
  );

  if (!response.isSuccess) {
    throw response.error;
  }
}

interface Variables {
  restaurantId: string;
  category: string;
}

export default function useRemoveMenuCategory() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, Variables, null>(
    async ({ restaurantId, category }) => {
      await removeMenuCategory(restaurantId, category);
    },
    {
      onSuccess: (_, { restaurantId }) => {
        queryCache.invalidateQueries(getMenuQueryKey(restaurantId));
      },
    }
  );
}
