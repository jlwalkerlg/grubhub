export interface UserDto {
  id: string;
  name: string;
  email: string;
  role: UserRole;
  mobileNumber?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  postcode?: string;
  restaurantId?: string;
  restaurantName?: string;
}

export type UserRole = "RestaurantManager" | "Customer" | "Admin";
