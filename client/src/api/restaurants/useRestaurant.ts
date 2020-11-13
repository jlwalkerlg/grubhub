import { useQuery } from "react-query";
import { Error } from "~/services/Error";
import api from "../Api";
import { RestaurantDto } from "./RestaurantDto";

export const getRestaurantQueryKey = (id: string) => ["restaurants", id];

const getRestaurant = async (id: string) => {
  const response = await api.get(`/restaurants/${id}`);

  if (!response.isSuccess) {
    throw response.error;
  }

  return response.data;
};

export default function useRestaurant(id: string) {
  return useQuery<RestaurantDto, Error>(getRestaurantQueryKey(id), () =>
    getRestaurant(id)
  );
}
