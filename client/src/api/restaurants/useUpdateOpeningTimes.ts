import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../api";
import { getRestaurantQueryKey } from "./useRestaurant";

export interface UpdateOpeningTimesCommand {
  mondayOpen: string;
  mondayClose: string;
  tuesdayOpen: string;
  tuesdayClose: string;
  wednesdayOpen: string;
  wednesdayClose: string;
  thursdayOpen: string;
  thursdayClose: string;
  fridayOpen: string;
  fridayClose: string;
  saturdayOpen: string;
  saturdayClose: string;
  sundayOpen: string;
  sundayClose: string;
}

async function updateOpeningTimes(
  restaurantId: string,
  command: UpdateOpeningTimesCommand
) {
  return api.put(`/restaurants/${restaurantId}/opening-times`, command);
}

export default function useUpdateOpeningTimes(restaurantId: string) {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, UpdateOpeningTimesCommand, null>(
    async (command) => {
      await updateOpeningTimes(restaurantId, command);
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getRestaurantQueryKey(restaurantId));
      },
    }
  );
}
