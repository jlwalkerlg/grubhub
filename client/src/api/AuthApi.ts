import Axios from "axios";
import Api, { AxiosApiResponse, ApiResponse } from "./Api";

interface LoginData {
  email: string;
  password: string;
}

interface LoginResponse {
  data: {
    id: string;
    name: string;
    email: string;
    password: string;
    role: string;
  };
}

class AuthApi extends Api {
  public login(data: LoginData): Promise<ApiResponse<LoginResponse>> {
    return Axios.post(this.getUrl("/login"), data)
      .then((response) => AxiosApiResponse.fromSuccess(response))
      .catch((error) => AxiosApiResponse.fromError(error));
  }
}

export default new AuthApi();
