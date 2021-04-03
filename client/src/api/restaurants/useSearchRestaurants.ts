import { useInfiniteQuery } from "react-query";
import api, { ApiError } from "../apii";
import { CuisineDto } from "../cuisines/useCuisines";
import { OpeningTimes } from "./useRestaurant";

export interface SearchRestaurantsResponse {
  restaurants: RestaurantModel[];
  count: number;
}

export interface RestaurantModel {
  id: string;
  name: string;
  latitude: number;
  longitude: number;
  openingTimes: OpeningTimes;
  deliveryFee: number;
  minimumDeliverySpend: number;
  maxDeliveryDistanceInKm: number;
  estimatedDeliveryTimeInMinutes: number;
  cuisines: CuisineDto[];
  thumbnail: string;
}

interface SearchRestaurantsQuery {
  postcode: string;
  sort_by?: string;
  cuisines?: string;
  perPage?: number;
}

function getSearchRestaurantsQueryKey(query: SearchRestaurantsQuery) {
  const params = Object.keys(query)
    .map((key) => `${key}=${query[key]}`)
    .join("&");
  return `restaurants?${params}`;
}

export default function useSearchRestaurants(query: SearchRestaurantsQuery) {
  return useInfiniteQuery<SearchRestaurantsResponse, ApiError>(
    getSearchRestaurantsQueryKey(query),
    async ({ pageParam: page }) => {
      const response = await api.get<SearchRestaurantsResponse>(
        "/restaurants",
        {
          params: {
            ...query,
            page,
          },
        }
      );
      return response.data;
    },
    {
      staleTime: 0,
      refetchOnMount: true,
      refetchOnReconnect: true,
      refetchOnWindowFocus: false,
      retry: false,
      getNextPageParam: (_, pages) => {
        const totalRestaurantsAvailable = pages[0].count;

        const numberOfRestaurantsLoaded = pages.reduce(
          (count, page) => count + page.restaurants.length,
          0
        );

        return numberOfRestaurantsLoaded === totalRestaurantsAvailable
          ? undefined
          : pages.length + 1;
      },
    }
  );
}
