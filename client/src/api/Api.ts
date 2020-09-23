import Axios, { AxiosRequestConfig, AxiosResponse } from "axios";

export default class Api {
  private baseUrl: string = process.env.NEXT_PUBLIC_API_BASE_URL;

  protected getUrl(path: string): string {
    if (path.startsWith("/")) {
      path = path.substr(1);
    }

    return `${this.baseUrl}/${path}`;
  }

  protected async get<T = null>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await Axios.get<T>(this.getUrl(url), {
        ...config,
        withCredentials: true,
      });

      return new ApiResponse<T>(response);
    } catch (e) {
      return new ApiResponse<T>(e.response);
    }
  }

  protected async post<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await Axios.post<T>(this.getUrl(url), data, {
        ...config,
        withCredentials: true,
      });

      return new ApiResponse<T>(response);
    } catch (e) {
      return new ApiResponse<T>(e.response);
    }
  }

  protected async put<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await Axios.put<T>(this.getUrl(url), data, {
        ...config,
        withCredentials: true,
      });

      return new ApiResponse<T>(response);
    } catch (e) {
      return new ApiResponse<T>(e.response);
    }
  }
}

export class ApiResponse<TData = null> {
  readonly data: TData;
  readonly error: string;
  readonly errors: { [key: string]: string };
  readonly statusCode: number;

  get isSuccess() {
    return this.statusCode >= 200 && this.statusCode < 300;
  }

  get isValidationError() {
    return this.statusCode === 422;
  }

  public constructor(response: AxiosResponse) {
    console.log(response);
    this.data = response.data?.data || null;
    this.error = response.data?.message || null;
    this.errors = response.data?.errors || null;
    this.statusCode = response.status;
  }
}
