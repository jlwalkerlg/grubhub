import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getBillingDetailsQueryKey } from "./useBillingDetails";

export default function useSetupBilling() {
  const cache = useQueryCache();

  return useMutation<string, ApiError, string, null>(
    async (restaurantId) => {
      const response = await Api.post(
        `/restaurants/${restaurantId}/billing/setup`
      );
      return response.data;
    },
    {
      onSuccess: (_, restaurantId) => {
        cache.invalidateQueries(getBillingDetailsQueryKey(restaurantId));
      },
    }
  );
}
