import { QueryConfig, useQuery } from "react-query";
import { sleep } from "~/services/utils";
import api, { ApiError } from "../Api";
import { RestaurantDto } from "./RestaurantDto";

export function getRestaurantQueryKey(id: string) {
  return ["restaurants", id];
}

async function getRestaurant(id: string) {
  await sleep(1000);
  const response = await api.get(`/restaurants/${id}`);
  return response.data;
}

export default function useRestaurant(
  id: string,
  config?: QueryConfig<RestaurantDto, ApiError>
) {
  return useQuery<RestaurantDto, ApiError>(
    getRestaurantQueryKey(id),
    () => getRestaurant(id),
    config
  );
}
