import axios from "axios";

import Api, { ApiResponse, AxiosApiResponse } from "./Api";

interface RegisterValues {
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
  async register(values: RegisterValues): Promise<ApiResponse> {
    return axios
      .post(this.getUrl("/restaurants/register"), values)
      .then((response) => AxiosApiResponse.fromSuccess(response))
      .catch((e) => AxiosApiResponse.fromError(e));
  }
}

export default new RestaurantsApi();
