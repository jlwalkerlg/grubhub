import { CuisineDto } from "../cuisines/CuisineDto";
import { MenuDto } from "../menu/MenuDto";
import { OpeningTimes } from "./OpeningTimes";

export interface RestaurantDto {
  id: string;
  managerId: string;
  name: string;
  phoneNumber: string;
  address: string;
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
}

type RestaurantStatus = "PendingApproval" | "Approved";
