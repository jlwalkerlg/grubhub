import axios from "axios";

import Api from "./Api";

interface RegisterValues {
  managerName: string;
  managerEmail: string;
  managerPassword: string;
  restaurantName: string;
  restaurantPhone: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  postCode: string;
}

class RestaurantsApi extends Api {
  async register(values: RegisterValues): Promise<null> {
    return axios.post(this.getUrl("/restaurants/register"), values);
  }
}

export default new RestaurantsApi();
