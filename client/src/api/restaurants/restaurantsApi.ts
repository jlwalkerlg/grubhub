import Api from "../Api";
import { RestaurantDto } from "./RestaurantDto";

export interface RegisterRestaurantCommand {
  managerName: string;
  managerEmail: string;
  managerPassword: string;
  restaurantName: string;
  restaurantPhoneNumber: string;
  addressLine1: string;
  addressLine2: string;
  town: string;
  postcode: string;
}

class RestaurantsApi extends Api {
  public getByManagerId(id: string) {
    return this.get<RestaurantDto>(`/managers/${id}/restaurant`);
  }

  public register(command: RegisterRestaurantCommand) {
    return this.post<null>("/restaurants/register", command);
  }
}

export default new RestaurantsApi();
