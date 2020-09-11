import { ApiResponse } from "~/api/Api";

export class Error {
  public constructor(readonly message: string) {}
}

export class ApiError extends Error {
  readonly statusCode: number;
  readonly errors: { [key: string]: string };

  get isValidationError() {
    return this.statusCode === 422;
  }

  public constructor(response: ApiResponse<any>) {
    super(response.error);

    this.statusCode = response.statusCode;
    this.errors = response.errors || null;
  }
}
