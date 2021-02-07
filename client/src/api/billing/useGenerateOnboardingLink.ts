import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";

export default function useGenerateOnboardingLink(
  restaurantId: string,
  config?: QueryConfig<string, ApiError>
) {
  return useQuery<string, ApiError>(
    ["onboarding-link", restaurantId],
    async () => {
      const response = await Api.get(
        `/restaurants/${restaurantId}/billing/onboarding/link`
      );
      return response.data;
    },
    {
      enabled: false,
      refetchOnMount: false,
      refetchOnReconnect: false,
      refetchOnWindowFocus: false,
      ...config,
    }
  );
}
