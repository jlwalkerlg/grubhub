export interface UserDto {
  id: string;
  name: string;
  email: string;
  password: string;
  role: UserRole;
  restaurantId: string;
  restaurantName: string;
}

export type UserRole = "RestaurantManager" | "Customer" | "Admin";
