import api from "../Api";
import { UserDto } from "./UserDto";

export interface LoginCommand {
  email: string;
  password: string;
}

export interface UpdateUserDetailsCommand {
  name: string;
  email: string;
}

class UserApi {
  public login(request: LoginCommand) {
    return api.post("/auth/login", request);
  }

  public logout() {
    return api.post("/auth/logout");
  }

  public getAuthData() {
    return api.get<UserDto>("/auth/user");
  }

  public updateDetails(request: UpdateUserDetailsCommand) {
    return api.put("/auth/user", request);
  }
}

export default new UserApi();
