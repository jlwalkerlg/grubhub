import Axios, { AxiosRequestConfig, AxiosResponse } from "axios";

export default class Api {
  private baseUrl: string = process.env.NEXT_PUBLIC_API_BASE_URL;

  protected getUrl(path: string): string {
    if (path.startsWith("/")) {
      path = path.substr(1);
    }

    return `${this.baseUrl}/${path}`;
  }

  protected async post<T = any>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await Axios.post<T>(url, data, config);

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
