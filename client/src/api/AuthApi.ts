import { UserDto } from "./dtos/UserDto";

import Api from "./Api";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  data: UserDto;
}

class AuthApi extends Api {
  public login(data: LoginRequest) {
    return this.post<LoginResponse>("/login", data);
  }
}

export default new AuthApi();
