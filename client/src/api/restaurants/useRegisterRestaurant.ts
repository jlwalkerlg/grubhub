import { useMutation } from "react-query";
import api, { ApiError } from "../apii";

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

export default function useRegisterRestaurant() {
  return useMutation<void, ApiError, RegisterRestaurantCommand, null>(
    async (command) => {
      await api.post("/restaurants/register", command);
    }
  );
}
