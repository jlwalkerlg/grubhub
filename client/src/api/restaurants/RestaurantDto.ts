export interface RestaurantDto {
  id: string;
  managerId: string;
  name: string;
  phoneNumber: string;
  address: string;
  latitude: number;
  longitude: number;
  status: RestaurantStatus;
}

type RestaurantStatus = "PendingApproval" | "Approved";
