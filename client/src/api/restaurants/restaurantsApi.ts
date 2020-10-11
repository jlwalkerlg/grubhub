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

export interface AddMenuCategoryRequest {
  name: string;
}

export interface AddMenuItemRequest {
  categoryName: string;
  itemName: string;
  description: string;
  price: number;
}

export interface UpdateMenuItemRequest {
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

  public addMenuCategory(
    restaurantId: string,
    request: AddMenuCategoryRequest
  ) {
    return api.post(`/restaurants/${restaurantId}/menu/categories`, request);
  }

  public removeMenuCategory(restaurantId: string, category: string) {
    return api.delete(
      `/restaurants/${restaurantId}/menu/categories/${category}`
    );
  }

  public addMenuItem(restaurantId: string, request: AddMenuItemRequest) {
    return api.post(`/restaurants/${restaurantId}/menu/items`, request);
  }

  public updateMenuItem(
    restaurantId: string,
    category: string,
    item: string,
    request: UpdateMenuItemRequest
  ) {
    return api.put(
      `/restaurants/${restaurantId}/menu/categories/${category}/items/${item}`,
      request
    );
  }

  public removeMenuItem(restaurantId: string, category: string, item: string) {
    return api.delete(
      `/restaurants/${restaurantId}/menu/categories/${category}/items/${item}`
    );
  }
}

export default new RestaurantsApi();
