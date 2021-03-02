import Axios, {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  Method,
} from "axios";

interface ProblemDetails {
  type?: string;
  title?: string;
  status?: string;
  detail?: string;
  instance?: string;
  errors?: { [key: string]: string[] };
}

export class ApiResult<T = void> {
  readonly data?: T;
  readonly statusCode: number;

  public constructor(response: AxiosResponse<T>) {
    this.data = response.data || undefined;
    this.statusCode = response.status;
  }
}

export class ApiError {
  readonly message: string;
  readonly statusCode: number;
  readonly errors?: { [key: string]: string[] };

  public constructor(response: AxiosResponse<ProblemDetails>) {
    this.statusCode = response?.status || 500;
    this.errors = response?.data?.errors;

    this.message =
      response?.data?.detail ??
      (this.statusCode === 404 ? "Resource not found." : "Server error.");
  }

  get isValidationError() {
    return this.statusCode === 422;
  }
}

class Api {
  private client: AxiosInstance = Axios.create({
    withCredentials: true,
    baseURL: process.env.NEXT_PUBLIC_API_BASE_URL,
    xsrfCookieName: "XSRF-TOKEN",
    xsrfHeaderName: "X-XSRF-TOKEN",
  });

  public async get<T = null>(url: string, config?: AxiosRequestConfig) {
    return this.request<T>(url, "GET", config);
  }

  public async post<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ) {
    return this.request<T>(url, "POST", {
      ...config,
      data,
    });
  }

  public async put<T = null>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ) {
    return this.request<T>(url, "PUT", {
      ...config,
      data,
    });
  }

  public async delete<T = null>(url: string, config?: AxiosRequestConfig) {
    return this.request<T>(url, "DELETE", {
      ...config,
    });
  }

  private async request<T = null>(
    url: string,
    method: Method,
    config?: AxiosRequestConfig
  ) {
    try {
      const response = await this.client.request<T>({
        ...config,
        url,
        method,
      });

      return new ApiResult<T>(response);
    } catch (e) {
      throw new ApiError(e.response);
    }
  }
}

export default new Api();
