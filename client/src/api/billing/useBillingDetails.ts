import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";

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
  config?: QueryConfig<BillingDetails, ApiError>
) {
  return useQuery<BillingDetails, ApiError>(
    getBillingDetailsQueryKey(restaurantId),
    async () => {
      const response = await Api.get(`/restaurants/${restaurantId}/billing`);
      return response.data;
    },
    config
  );
}
