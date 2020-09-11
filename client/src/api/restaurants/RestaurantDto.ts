export interface RestaurantDto {
  id: string;
  managerId: string;
  name: string;
  phoneNumber: string;
  addressLine1: string;
  addressLine2: string;
  town: string;
  postcode: string;
  latitude: number;
  longitude: number;
  status: RestaurantStatus;
}

type RestaurantStatus = "Pending" | "Accepted";
