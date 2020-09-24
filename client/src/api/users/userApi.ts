import Api from "../Api";
import { UserDto } from "./UserDto";

export interface LoginCommand {
  email: string;
  password: string;
}

export interface UpdateUserDetailsCommand {
  name: string;
  email: string;
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

  public updateDetails(request: UpdateUserDetailsCommand) {
    return this.put("/auth/user", request);
  }
}

export default new UserApi();
