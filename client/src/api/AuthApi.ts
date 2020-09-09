import { UserDto } from "./dtos/UserDto";

import Api from "./Api";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  data: UserDto;
}

export interface GetAuthUserResponse {
  data: UserDto;
}

class AuthApi extends Api {
  public login(data: LoginRequest) {
    return this.post<LoginResponse>("/api/login", data);
  }

  public logout() {
    return this.post<null>("/api/logout");
  }
}

export default new AuthApi();
