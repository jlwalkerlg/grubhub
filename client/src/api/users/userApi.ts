import Api from "../Api";
import { UserDto } from "./UserDto";

export interface LoginCommand {
  email: string;
  password: string;
}

class UserApi extends Api {
  public login(request: LoginCommand) {
    return this.post("/auth/login", request);
  }

  public logout() {
    return this.post("/auth/logout");
  }

  public getAuthData() {
    return this.get<UserDto>("/auth/user");
  }
}

export default new UserApi();
