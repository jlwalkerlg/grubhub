import Api from "./Api";

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
    return this.post(this.getUrl("/restaurants/register"), request);
  }
}

export default new RestaurantsApi();
