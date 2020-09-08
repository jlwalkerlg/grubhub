import { ApiResponse } from "~/api/Api";

export class Error {
  public constructor(readonly message: string) {}
}

export class ApiError extends Error {
  readonly statusCode: number;
  readonly validationErrors: { [key: string]: string };

  get isValidationError() {
    return this.statusCode === 422;
  }

  public constructor(response: ApiResponse) {
    super(response.data.error);

    this.statusCode = response.statusCode;
    this.validationErrors = response.data.errors;
  }
}
