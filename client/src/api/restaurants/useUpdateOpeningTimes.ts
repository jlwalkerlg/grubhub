import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
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
  return Api.put(`/restaurants/${restaurantId}/opening-times`, command);
}

export default function useUpdateOpeningTimes(restaurantId: string) {
  const cache = useQueryCache();

  return useMutation<void, ApiError, UpdateOpeningTimesCommand, null>(
    async (command) => {
      await updateOpeningTimes(restaurantId, command);
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(getRestaurantQueryKey(restaurantId));
      },
    }
  );
}
