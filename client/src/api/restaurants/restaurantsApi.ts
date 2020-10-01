import Api from "../Api";
import { MenuDto } from "./MenuDto";
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

export interface UpdateRestaurantDetailsCommand {
  name: string;
  phoneNumber: string;
}

class RestaurantsApi extends Api {
  public getById(id: string) {
    return this.get<RestaurantDto>(`/restaurants/${id}`);
  }

  public getMenuByRestaurantId(restaurantId: string) {
    return this.get<MenuDto>(`/restaurants/${restaurantId}/menu`);
  }

  public register(command: RegisterRestaurantCommand) {
    return this.post<null>("/restaurants/register", command);
  }

  public updateDetails(id: string, command: UpdateRestaurantDetailsCommand) {
    return this.put<null>(`/restaurants/${id}`, command);
  }
}

export default new RestaurantsApi();
