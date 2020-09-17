import { UserDto } from "./UserDto";

import Api from "../Api";
import { RestaurantDto } from "../restaurants/RestaurantDto";

export interface LoginCommand {
  email: string;
  password: string;
}

export interface AuthData {
  user: UserDto;
  restaurant: RestaurantDto;
}

class UserApi extends Api {
  public login(request: LoginCommand) {
    return this.post("/auth/login", request);
  }

  public logout() {
    return this.post("/auth/logout");
  }

  public getAuthData() {
    return this.get<AuthData>("/auth/data");
  }
}

export default new UserApi();
