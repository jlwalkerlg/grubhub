export interface UserDto {
  id: string;
  name: string;
  email: string;
  password: string;
  role: UserRole;
}

export type UserRole = "RestaurantManager" | "Customer" | "Admin";
