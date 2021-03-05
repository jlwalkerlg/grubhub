import Axios, {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  Method,
} from "axios";

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

  public constructor(response: AxiosResponse<ProblemDetails>) {
    this.type = response.data.type;
    this.title = response.data.title;
    this.status = response.data.status;
    this.traceId = response.data.traceId;
    this.detail = response.data.detail;
    this.instance = response.data.instance;
    this.errors = response.data.errors;
  }

  get isValidationError() {
    return this.status === 422;
  }

  get message() {
    return this.detail;
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
