import Axios, {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  Method,
} from "axios";
import { Error } from "~/lib/Error";

class Api {
  private client: AxiosInstance = Axios.create({
    withCredentials: true,
    baseURL: process.env.NEXT_PUBLIC_API_BASE_URL,
  });

  public async get<T = null>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    return this.request(url, "GET", config);
  }

  public async post<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    return this.request(url, "POST", {
      ...config,
      data,
    });
  }

  public async put<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    return this.request(url, "PUT", {
      ...config,
      data,
    });
  }

  private async request<T = null>(
    url: string,
    method: Method,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.request({
        ...config,
        url,
        method,
      });

      return new ApiResponse<T>(response);
    } catch (e) {
      return new ApiResponse<T>(e.response);
    }
  }
}

export class ApiResponse<TData = null> {
  readonly data: TData = null;
  readonly error: Error = null;
  readonly statusCode: number;

  get isSuccess() {
    return this.isSuccessStatusCode(this.statusCode);
  }

  private isSuccessStatusCode(statusCode: number) {
    return statusCode >= 200 && statusCode < 300;
  }

  public constructor(response: AxiosResponse) {
    this.data = response.data?.data || null;
    this.statusCode = response.status;

    if (!this.isSuccessStatusCode(response.status)) {
      this.error = new Error(
        response.data?.message || response.statusText,
        response.status,
        response.data?.errors
      );
    }
  }
}

export default new Api();
