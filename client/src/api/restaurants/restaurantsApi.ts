import Api from "../Api";
import { RestaurantDto } from "./RestaurantDto";
import { DataEnvelope } from "../DataEnvelope";

export interface RegisterRequest {
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
  public register(request: RegisterRequest) {
    return this.post<null>("/restaurants/register", request);
  }

  public getAuthUserRestaurantDetails() {
    return this.get<DataEnvelope<RestaurantDto>>("/auth/restaurant/details");
  }
}

export default new RestaurantsApi();
