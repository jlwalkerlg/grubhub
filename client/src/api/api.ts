import axios, {
  AxiosError,
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  Method,
} from "axios";
import { API_BASE_URL } from "~/config";

interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  traceId?: string;
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

export class ApiError extends Error {
  problem?: ProblemDetails;
  status: number;

  public constructor(error: AxiosError<any>) {
    const problem = error.response?.data as ProblemDetails;
    super(problem?.detail ?? error.message);
    this.problem = problem;
    this.status = error.response?.status ?? 500;
  }

  get isValidationError() {
    return this.status === 422;
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
      throw axios.isAxiosError(e) ? new ApiError(e) : e;
    }
  }

  public isApiError(payload: any): payload is ApiError {
    return payload instanceof ApiError;
  }
}

export default new Api();
