import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
import { getRestaurantQueryKey } from "./useRestaurant";

export interface UpdateCuisinesRequest {
  cuisines: string[];
}

async function updateCuisines(
  restaurantId: string,
  request: UpdateCuisinesRequest
) {
  return api.put(`/restaurants/${restaurantId}/cuisines`, request);
}

export default function useUpdateCuisines(restaurantId: string) {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, UpdateCuisinesRequest, null>(
    async (command) => {
      await updateCuisines(restaurantId, command);
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getRestaurantQueryKey(restaurantId));
      },
    }
  );
}
