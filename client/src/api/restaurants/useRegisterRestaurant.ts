import { useMutation } from "react-query";
import api, { ApiError } from "../api";

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

interface RegisterRestaurantResponse {
  xsrfToken: string;
}

export default function useRegisterRestaurant() {
  return useMutation<void, ApiError, RegisterRestaurantCommand, null>(
    async (command) => {
      const {
        data: { xsrfToken },
      } = await api.post<RegisterRestaurantResponse>(
        "/restaurants/register",
        command
      );

      localStorage.setItem("XSRF-TOKEN", xsrfToken);
    }
  );
}
