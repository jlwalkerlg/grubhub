import { useMutation } from "react-query";
import Api, { ApiError } from "../api";

export interface RegisterRestaurantCommand {
  managerFirstName: string;
  managerLastName: string;
  managerEmail: string;
  managerPassword: string;
  restaurantName: string;
  restaurantPhoneNumber: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  postcode: string;
}

async function registerRestaurant(command: RegisterRestaurantCommand) {
  await Api.post("/restaurants/register", command);
}

export default function useRegisterRestaurant() {
  return useMutation<void, ApiError, RegisterRestaurantCommand, null>(
    registerRestaurant
  );
}
