import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { getRestaurantQueryKey } from "./useRestaurant";

export interface UpdateRestaurantDetailsRequest {
  name: string;
  phoneNumber: string;
}

async function updateRestaurantDetails(
  id: string,
  request: UpdateRestaurantDetailsRequest
) {
  const response = await Api.put(`/restaurants/${id}`, request);

  if (!response.isSuccess) {
    throw response.error;
  }
}

interface Variables {
  id: string;
  request: UpdateRestaurantDetailsRequest;
}

export default function useUpdateRestaurantDetails() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, Variables, null>(
    async ({ id, request }) => {
      await updateRestaurantDetails(id, request);
    },
    {
      onSuccess: (_, { id }) => {
        queryCache.invalidateQueries(getRestaurantQueryKey(id));
      },
    }
  );
}
