import { useMutation } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";

export interface RegisterRestaurantCommand {
  managerName: string;
  managerEmail: string;
  managerPassword: string;
  restaurantName: string;
  restaurantPhoneNumber: string;
  address: string;
}

async function registerRestaurant(command: RegisterRestaurantCommand) {
  const response = await Api.post("/restaurants/register", command);

  if (!response.isSuccess) {
    throw response.error;
  }
}

export default function useRegisterRestaurant() {
  return useMutation<void, Error, RegisterRestaurantCommand, null>(
    async (command) => {
      await registerRestaurant(command);
    }
  );
}
