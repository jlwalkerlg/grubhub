import { useMutation } from "react-query";
import Api, { ApiError } from "../Api";

export interface RegisterRestaurantCommand {
  managerName: string;
  managerEmail: string;
  managerPassword: string;
  restaurantName: string;
  restaurantPhoneNumber: string;
  address: string;
}

async function registerRestaurant(command: RegisterRestaurantCommand) {
  await Api.post("/restaurants/register", command);
}

export default function useRegisterRestaurant() {
  return useMutation<void, ApiError, RegisterRestaurantCommand, null>(
    registerRestaurant
  );
}
