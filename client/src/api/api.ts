import axios, {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  Method,
} from "axios";
import { API_BASE_URL } from "~/config";

interface ProblemDetails {
  type: string;
  title: string;
  status: number;
  traceId: string;
  detail?: string;
  instance?: string;
  errors?: { [key: string]: string[] };
}

export class ApiResult<T = void> {
  readonly data?: T;
  readonly status: number;

  public constructor(response: AxiosResponse<T>) {
    this.data = response.data || undefined;
    this.status = response.status;
  }
}

export class ApiError {
  readonly type: string;
  readonly title: string;
  readonly status: number;
  readonly traceId: string;
  readonly detail?: string;
  readonly instance?: string;
  readonly errors?: { [key: string]: string[] };

  public constructor(error: Error) {
    if (axios.isAxiosError(error) && error.response?.data) {
      const details = error.response.data as ProblemDetails;

      this.type = details.type;
      this.title = details.title;
      this.status = details.status;
      this.traceId = details.traceId;
      this.detail = details.detail;
      this.instance = details.instance;
      this.errors = details.errors;
    } else {
      this.detail = error.message;
    }
  }

  get isValidationError() {
    return this.status === 422;
  }

  get message() {
    return this.detail;
  }
}

class Api {
  private client: AxiosInstance = axios.create({
    withCredentials: true,
    baseURL: API_BASE_URL,
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
        headers: {
          ...(config?.headers ?? {}),
          "X-XSRF-TOKEN": localStorage.getItem("XSRF-TOKEN") ?? undefined,
        },
        url,
        method,
      });

      return new ApiResult<T>(response);
    } catch (e) {
      throw new ApiError(e);
    }
  }

  public isApiError(payload: any): payload is ApiError {
    return payload instanceof ApiError;
  }
}

export default new Api();
