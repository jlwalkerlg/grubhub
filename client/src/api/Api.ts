import { AxiosResponse, AxiosError } from "axios";

export default class Api {
  private baseUrl: string = process.env.NEXT_PUBLIC_API_BASE_URL;

  protected getUrl(path: string): string {
    if (path.startsWith("/")) {
      path = path.substr(1);
    }

    return `${this.baseUrl}/${path}`;
  }
}

export interface ValidationErrors {
  [key: string]: string;
}

export interface ApiResponse<TData = any> {
  data: TData;
  statusCode: number;
  error: string;
  validationErrors: ValidationErrors;
  isSuccess: boolean;
  isValidationError: boolean;
}

export class AxiosApiResponse<TData = any> implements ApiResponse<TData> {
  get isValidationError() {
    return this.statusCode === 422;
  }

  get isSuccess() {
    return this.statusCode >= 200 && this.statusCode < 300;
  }

  readonly statusCode: number;
  readonly data: TData;
  readonly error: string;
  readonly validationErrors: ValidationErrors;

  private constructor(response: AxiosResponse, error: AxiosError) {
    if (response !== null) {
      this.statusCode = response.status;
      this.data = response.data;
    }

    if (error !== null) {
      this.statusCode = error.response.status;
      this.error = error.response.data?.message;
      this.validationErrors = error.response.data?.errors;
    }
  }

  public static fromSuccess<TData = any>(
    response: AxiosResponse<TData>
  ): AxiosApiResponse<TData> {
    return new AxiosApiResponse<TData>(response, null);
  }

  public static fromError<TData = any>(
    error: AxiosError<TData>
  ): AxiosApiResponse<TData> {
    return new AxiosApiResponse<TData>(null, error);
  }
}
