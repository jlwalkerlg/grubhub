import Axios, { AxiosRequestConfig, AxiosResponse } from "axios";

export default class Api {
  private baseUrl: string = process.env.NEXT_PUBLIC_API_BASE_URL;

  private static config: AxiosRequestConfig = {
    headers: {},
  };

  private static addCookie = (name: string, value: string) => {
    Api.config.withCredentials = true;
    Api.config.headers["Cookie"] = `${name}=${value}`;
  };

  public static addAuthToken = (token: string) => {
    Api.addCookie("auth_token", token);
  };

  private static mergeConfig = (config: AxiosRequestConfig = {}) => {
    return { ...Api.config, ...config };
  };

  protected getUrl(path: string): string {
    if (path.startsWith("/")) {
      path = path.substr(1);
    }

    return `${this.baseUrl}/${path}`;
  }

  protected async get<T = any>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await Axios.get<T>(url, Api.mergeConfig(config));

      return new ApiResponse<T>(response);
    } catch (e) {
      return new ApiResponse<T>(e.response);
    }
  }

  protected async post<T = any>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await Axios.post<T>(url, data, Api.mergeConfig(config));

      return new ApiResponse<T>(response);
    } catch (e) {
      return new ApiResponse<T>(e.response);
    }
  }
}

export class ApiResponse<TData = any> {
  readonly data: TData;
  readonly statusCode: number;
  readonly error: string;
  readonly validationErrors: { [key: string]: string };

  get isSuccess() {
    return this.statusCode >= 200 && this.statusCode < 300;
  }

  get isValidationError() {
    return this.statusCode === 422;
  }

  public constructor(response: AxiosResponse) {
    this.data = response.data || null;
    this.statusCode = response.status;
    this.error = response.data?.message || null;
    this.validationErrors = response.data.errors || null;
  }
}
