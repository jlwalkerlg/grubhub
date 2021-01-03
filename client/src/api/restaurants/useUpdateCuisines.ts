import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getRestaurantQueryKey } from "./useRestaurant";

export interface UpdateCuisinesRequest {
  cuisines: string[];
}

async function updateCuisines(
  restaurantId: string,
  request: UpdateCuisinesRequest
) {
  return Api.put(`/restaurants/${restaurantId}/cuisines`, request);
}

export default function useUpdateCuisines(restaurantId: string) {
  const cache = useQueryCache();

  return useMutation<void, ApiError, UpdateCuisinesRequest, null>(
    async (command) => {
      await updateCuisines(restaurantId, command);
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(getRestaurantQueryKey(restaurantId));
      },
    }
  );
}
