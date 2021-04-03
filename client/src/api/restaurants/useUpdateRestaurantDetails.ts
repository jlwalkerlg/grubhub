import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
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

  await api.put(`/restaurants/${id}`, data);
}

export default function useUpdateRestaurantDetails() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, UpdateRestaurantDetailsCommand, null>(
    updateRestaurantDetails,
    {
      onSuccess: (_, command) => {
        queryClient.invalidateQueries(getRestaurantQueryKey(command.id));
      },
    }
  );
}
