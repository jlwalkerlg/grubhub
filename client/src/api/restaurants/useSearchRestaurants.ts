import { useQuery } from "react-query";
import { sleep } from "~/services/utils";
import Api, { ApiError } from "../Api";
import { CuisineDto } from "../cuisines/CuisineDto";
import { OpeningTimes } from "./OpeningTimes";

export interface RestaurantSearchResult {
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
}

interface SearchRestaurantsQuery {
  postcode: string;
  sort_by?: string;
  cuisines?: string;
}

async function searchRestaurants(query: SearchRestaurantsQuery) {
  await sleep(1000);

  const params = Object.keys(query)
    .map((key) => `${key}=${query[key]}`)
    .join("&");
  const response = await Api.get<Array<RestaurantSearchResult>>(
    `/restaurants?${params}`
  );
  return response.data;
}

function getSearchRestaurantsQueryKey(query: SearchRestaurantsQuery) {
  const params = Object.keys(query)
    .map((key) => `${key}=${query[key]}`)
    .join("&");
  return `restaurants?${params}`;
}

export default function useSearchRestaurants(query: SearchRestaurantsQuery) {
  return useQuery<Array<RestaurantSearchResult>, ApiError>(
    getSearchRestaurantsQueryKey(query),
    () => searchRestaurants(query),
    {
      staleTime: 0,
      cacheTime: 0,
      refetchOnMount: false,
      refetchOnReconnect: true,
      refetchOnWindowFocus: false,
      retry: false,
    }
  );
}
