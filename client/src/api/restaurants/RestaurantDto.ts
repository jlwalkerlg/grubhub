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
}

type RestaurantStatus = "PendingApproval" | "Approved";

interface OpeningTimes {
  monday: OpeningHours;
  tuesday: OpeningHours;
  wednesday: OpeningHours;
  thursday: OpeningHours;
  friday: OpeningHours;
  saturday: OpeningHours;
  sunday: OpeningHours;
}

interface OpeningHours {
  open: string;
  close: string;
}
