import Axios, {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  Method,
} from "axios";

interface DataEnvelope<T> {
  data: T;
}

interface ErrorEnvelope {
  message: string;
  errors: { [key: string]: string } | null;
}

export class ApiResult<T = void> {
  readonly data?: T;
  readonly statusCode: number;

  public constructor(response: AxiosResponse<DataEnvelope<T> | null>) {
    this.data = response.data.data;
    this.statusCode = response.status;
  }
}

export class ApiError {
  readonly message: string;
  readonly errors: { [key: string]: string };
  readonly statusCode: number;

  public constructor(response: AxiosResponse<ErrorEnvelope>) {
    this.message = response.data.message;
    this.errors = response.data.errors;
    this.statusCode = response.status;
  }

  get isValidationError() {
    return this.statusCode === 422;
  }
}

class Api {
  private client: AxiosInstance = Axios.create({
    withCredentials: true,
    baseURL: process.env.NEXT_PUBLIC_API_BASE_URL,
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
      const response = await this.client.request<DataEnvelope<T> | null>({
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
