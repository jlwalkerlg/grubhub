import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { getMenuQueryKey } from "../menu/useMenu";

export interface RenameMenuCategoryRequest {
  newName: string;
}

async function renameMenuCategory(
  restaurantId: string,
  oldName: string,
  request: RenameMenuCategoryRequest
) {
  const response = await Api.put(
    `/restaurants/${restaurantId}/menu/categories/${oldName}`,
    request
  );

  if (!response.isSuccess) {
    throw response.error;
  }
}

interface Variables {
  restaurantId: string;
  oldName: string;
  request: RenameMenuCategoryRequest;
}

export default function useRenameMenuCategory() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, Variables, null>(
    async ({ restaurantId, oldName, request }) => {
      await renameMenuCategory(restaurantId, oldName, request);
    },
    {
      onSuccess: (_, { restaurantId }) => {
        queryCache.invalidateQueries(getMenuQueryKey(restaurantId));
      },
    }
  );
}
