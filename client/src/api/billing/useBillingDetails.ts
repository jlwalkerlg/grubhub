import { useQuery, UseQueryOptions } from "react-query";
import api, { ApiError } from "../api";

interface BillingDetails {
  id: string;
  restaurantId: string;
  isBillingEnabled: boolean;
}

export function getBillingDetailsQueryKey(restaurantId: string) {
  return ["billing-details", restaurantId];
}

export default function useBillingDetails(
  restaurantId: string,
  config?: UseQueryOptions<BillingDetails, ApiError>
) {
  return useQuery<BillingDetails, ApiError>(
    getBillingDetailsQueryKey(restaurantId),
    async () => {
      const response = await api.get(`/restaurants/${restaurantId}/billing`);
      return response.data;
    },
    config
  );
}
