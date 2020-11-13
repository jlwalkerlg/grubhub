import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { MenuDto } from "../menu/MenuDto";
import { getMenuQueryKey } from "../menu/useMenu";

export interface AddMenuItemRequest {
  categoryName: string;
  itemName: string;
  description: string;
  price: number;
}

async function addMenuItem(restaurantId: string, request: AddMenuItemRequest) {
  const response = await Api.post(
    `/restaurants/${restaurantId}/menu/items`,
    request
  );

  if (!response.isSuccess) {
    throw response.error;
  }
}

interface Variables {
  restaurantId: string;
  request: AddMenuItemRequest;
}

export default function useAddMenuItem() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, Variables, null>(
    async ({ restaurantId, request }) => {
      await addMenuItem(restaurantId, request);
    },
    {
      onSuccess: (_, { restaurantId, request }) => {
        const menu = queryCache.getQueryData<MenuDto>(
          getMenuQueryKey(restaurantId)
        );

        if (menu !== undefined) {
          queryCache.setQueryData<MenuDto>(getMenuQueryKey(restaurantId), {
            ...menu,
            categories: menu.categories.map((x) =>
              x.name === request.categoryName
                ? {
                    ...x,
                    items: [
                      ...x.items,
                      {
                        name: request.itemName,
                        description: request.description,
                        price: request.price,
                      },
                    ],
                  }
                : x
            ),
          });
        }

        queryCache.invalidateQueries(getMenuQueryKey(restaurantId));
      },
    }
  );
}
