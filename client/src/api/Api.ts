import Axios, { AxiosRequestConfig, AxiosResponse, Method } from "axios";
import { Error } from "~/lib/Error";

export default class Api {
  private baseUrl: string = process.env.NEXT_PUBLIC_API_BASE_URL;

  private getUrl(path: string): string {
    if (path.startsWith("/")) {
      path = path.substr(1);
    }

    return `${this.baseUrl}/${path}`;
  }

  protected async get<T = null>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    return this.request(url, "GET");
  }

  protected async post<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    return this.request(url, "POST", {
      data,
    });
  }

  protected async put<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    return this.request(url, "PUT", {
      data,
    });
  }

  private async request<T = null>(
    url: string,
    method: Method,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await Axios.request({
        url: this.getUrl(url),
        method,
        withCredentials: true,
        ...config,
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
