import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getRestaurantQueryKey } from "./useRestaurant";

export interface UpdateRestaurantDetailsCommand {
  id: string;
  name: string;
  description?: string;
  phoneNumber: string;
  deliveryFee: number;
  minimumDeliverySpend: number;
  maxDeliveryDistanceInKm: number;
  estimatedDeliveryTimeInMinutes: number;
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
