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

export interface UpdateRestaurantDetailsRequest {
  name: string;
  phoneNumber: string;
}

export interface AddMenuItemRequest {
  category: string;
  name: string;
  description: string;
  price: number;
}

class RestaurantsApi {
  public getById(id: string) {
    return api.get<RestaurantDto>(`/restaurants/${id}`);
  }

  public getMenuByRestaurantId(restaurantId: string) {
    return api.get<MenuDto>(`/restaurants/${restaurantId}/menu`);
  }

  public register(command: RegisterRestaurantCommand) {
    return api.post("/restaurants/register", command);
  }

  public updateDetails(id: string, command: UpdateRestaurantDetailsRequest) {
    return api.put(`/restaurants/${id}`, command);
  }

  public addMenuItem(menuId: string, request: AddMenuItemRequest) {
    return api.post<string>(`/menus/${menuId}/items`, request);
  }
}

export default new RestaurantsApi();
