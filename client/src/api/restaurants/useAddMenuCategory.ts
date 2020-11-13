import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { MenuDto } from "../menu/MenuDto";
import { getMenuQueryKey } from "../menu/useMenu";

export interface AddMenuCategoryRequest {
  name: string;
}

async function addMenuCategory(
  restaurantId: string,
  request: AddMenuCategoryRequest
) {
  const response = await Api.post(
    `/restaurants/${restaurantId}/menu/categories`,
    request
  );

  if (!response.isSuccess) {
    throw response.error;
  }
}

interface Variables {
  restaurantId: string;
  request: AddMenuCategoryRequest;
}

export default function useAddMenuCategory() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, Variables, null>(
    async ({ restaurantId, request }) => {
      await addMenuCategory(restaurantId, request);
    },
    {
      onSuccess: (_, { restaurantId, request }) => {
        const menu = queryCache.getQueryData<MenuDto>(
          getMenuQueryKey(restaurantId)
        );

        if (menu !== undefined) {
          queryCache.setQueryData<MenuDto>(getMenuQueryKey(restaurantId), {
            ...menu,
            categories: [
              ...menu.categories,
              {
                name: request.name,
                items: [],
              },
            ],
          });
        }

        queryCache.invalidateQueries(getMenuQueryKey(restaurantId));
      },
    }
  );
}
