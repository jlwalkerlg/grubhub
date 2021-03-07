export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
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
