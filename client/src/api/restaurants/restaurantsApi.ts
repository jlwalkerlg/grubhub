import Api from "../Api";

export interface RegisterRestaurantCommand {
  managerName: string;
  managerEmail: string;
  managerPassword: string;
  restaurantName: string;
  restaurantPhoneNumber: string;
  addressLine1: string;
  addressLine2: string;
  town: string;
  postCode: string;
}

class RestaurantsApi extends Api {
  public register(command: RegisterRestaurantCommand) {
    return this.post<null>("/restaurants/register", command);
  }
}

export default new RestaurantsApi();
