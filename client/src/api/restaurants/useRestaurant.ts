import { useQuery, UseQueryOptions } from "react-query";
import api, { ApiError } from "../apii";
import { CuisineDto } from "../cuisines/useCuisines";

export interface RestaurantDto {
  id: string;
  managerId: string;
  name: string;
  description?: string;
  phoneNumber: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  postcode: string;
  latitude: number;
  longitude: number;
  status: RestaurantStatus;
  openingTimes: OpeningTimes;
  deliveryFee: number;
  minimumDeliverySpend: number;
  maxDeliveryDistanceInKm: number;
  estimatedDeliveryTimeInMinutes: number;
  menu: MenuDto;
  cuisines: CuisineDto[];
  thumbnail: string;
  banner: string;
}

export type RestaurantStatus = "PendingApproval" | "Approved";

export interface OpeningTimes {
  monday: OpeningHours;
  tuesday: OpeningHours;
  wednesday: OpeningHours;
  thursday: OpeningHours;
  friday: OpeningHours;
  saturday: OpeningHours;
  sunday: OpeningHours;
}

export interface OpeningHours {
  open: string;
  close: string;
}

export interface MenuDto {
  restaurantId: string;
  categories: MenuCategoryDto[];
}

export interface MenuCategoryDto {
  id: string;
  name: string;
  items: MenuItemDto[];
}

export interface MenuItemDto {
  id: string;
  name: string;
  description?: string;
  price: number;
}

export function getRestaurantQueryKey(id: string) {
  return ["restaurants", id];
}

async function getRestaurant(id: string) {
  const response = await api.get(`/restaurants/${id}`);
  return response.data;
}

export default function useRestaurant(
  id: string,
  config?: UseQueryOptions<RestaurantDto, ApiError>
) {
  return useQuery<RestaurantDto, ApiError>(
    getRestaurantQueryKey(id),
    () => getRestaurant(id),
    config
  );
}
