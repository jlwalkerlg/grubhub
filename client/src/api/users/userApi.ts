import { UserDto } from "./UserDto";

import Api from "../Api";
import { RestaurantDto } from "../restaurants/RestaurantDto";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthData {
  user: UserDto;
  restaurant: RestaurantDto;
}

class UserApi extends Api {
  public login(request: LoginRequest) {
    return this.post<AuthData>("/login", request);
  }

  public logout() {
    return this.post("/logout");
  }
}

export default new UserApi();
