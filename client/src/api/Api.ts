import { AxiosResponse } from "axios";

export default class Api {
  // TODOO: pull api url from config
  private baseUrl: string = "http://localhost:5000";

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

export interface ApiResponse {
  data: any;
  statusCode: number;
  error: string;
  validationErrors: ValidationErrors;
  isSuccess: boolean;
  isValidationError: boolean;
}

export class AxiosApiResponse implements ApiResponse {
  get isValidationError() {
    return this.statusCode === 422;
  }

  get isSuccess() {
    return this.statusCode >= 200 && this.statusCode < 300;
  }

  readonly statusCode: number;
  readonly data: any;
  readonly error: string;
  readonly validationErrors: ValidationErrors;

  constructor(response: AxiosResponse) {
    this.statusCode = response.status;
    this.data = response.data;
    this.error = response.data?.message;
    this.validationErrors = response.data?.errors;
  }
}
