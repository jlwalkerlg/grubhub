import { UserDto } from "./dtos/UserDto";

import Api from "./Api";
import { RestaurantDto } from "./dtos/RestaurantDto";
import { DataEnvelope } from "./dtos/DataEnvelope";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthData {
  user: UserDto;
  restaurant: RestaurantDto;
}

export type GetAuthUserResponse = DataEnvelope<UserDto>;

class AuthApi extends Api {
  public login(data: LoginRequest) {
    return this.post<DataEnvelope<AuthData>>("/api/login", data);
  }

  public logout() {
    return this.post<null>("/api/logout");
  }
}

export default new AuthApi();
