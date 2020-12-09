import { useQuery } from "react-query";
import Api, { ApiError } from "../Api";
import { RestaurantDto } from "./RestaurantDto";

async function searchRestaurants(postcode: string) {
  const response = await Api.get<Array<RestaurantDto>>(
    `/restaurants?postcode=${postcode}`
  );
  return response.data;
}

function getSearchRestaurantsQueryKey(postcode: string) {
  return `restaurants.${postcode}`;
}

export default function useSearchRestaurants(postcode: string) {
  return useQuery<Array<RestaurantDto>, ApiError>(
    getSearchRestaurantsQueryKey(postcode),
    () => searchRestaurants(postcode),
    {
      staleTime: 0,
      refetchOnMount: false,
      refetchOnReconnect: true,
      refetchOnWindowFocus: false,
      retry: false,
    }
  );
}
