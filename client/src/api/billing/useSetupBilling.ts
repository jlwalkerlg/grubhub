import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../api";
import { getBillingDetailsQueryKey } from "./useBillingDetails";

export default function useSetupBilling() {
  const queryClient = useQueryClient();

  return useMutation<string, ApiError, string, null>(
    async (restaurantId) => {
      const response = await api.post(
        `/restaurants/${restaurantId}/billing/setup`
      );
      return response.data;
    },
    {
      onSuccess: (_, restaurantId) => {
        queryClient.invalidateQueries(getBillingDetailsQueryKey(restaurantId));
      },
    }
  );
}
