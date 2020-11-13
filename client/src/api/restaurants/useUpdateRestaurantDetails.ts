import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getRestaurantQueryKey } from "./useRestaurant";

export interface UpdateRestaurantDetailsCommand {
  id: string;
  name: string;
  phoneNumber: string;
}

async function updateRestaurantDetails(
  command: UpdateRestaurantDetailsCommand
) {
  const { id, ...data } = command;

  await Api.put(`/restaurants/${id}`, data);
}

export default function useUpdateRestaurantDetails() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, UpdateRestaurantDetailsCommand, null>(
    updateRestaurantDetails,
    {
      onSuccess: (_, command) => {
        queryCache.invalidateQueries(getRestaurantQueryKey(command.id));
      },
    }
  );
}
