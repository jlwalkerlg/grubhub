import api from "../Api";
import { MenuDto } from "./MenuDto";
import { RestaurantDto } from "./RestaurantDto";

export interface RegisterRestaurantCommand {
  managerName: string;
  managerEmail: string;
  managerPassword: string;
  restaurantName: string;
  restaurantPhoneNumber: string;
  address: string;
}

export interface UpdateRestaurantDetailsCommand {
  name: string;
  phoneNumber: string;
}

class RestaurantsApi {
  public getById(id: string) {
    return api.get<RestaurantDto>(`/restaurants/${id}`);
  }

  public getMenuByRestaurantId(restaurantId: string) {
    return api.get<MenuDto>(`/restaurants/${restaurantId}/menu`);
  }

  public register(command: RegisterRestaurantCommand) {
    return api.post<null>("/restaurants/register", command);
  }

  public updateDetails(id: string, command: UpdateRestaurantDetailsCommand) {
    return api.put<null>(`/restaurants/${id}`, command);
  }
}

export default new RestaurantsApi();
