import { CuisineDto } from "../cuisines/CuisineDto";
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
  cuisines: CuisineDto[];
}

type RestaurantStatus = "PendingApproval" | "Approved";
