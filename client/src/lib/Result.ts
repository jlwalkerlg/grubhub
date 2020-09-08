import { Error } from "./Error";

export class Result<T, TError extends Error = Error> {
  readonly data?: T;
  readonly error?: TError;

  get isSuccess() {
    return this.error === null;
  }

  public constructor(data: T, error: TError) {
    this.data = data;
    this.error = error;
  }

  public static ok(): Result<null>;
  public static ok<T>(data: T): Result<T>;
  public static ok<T, TError extends Error = Error>(data: T): Result<T, TError>;
  public static ok<T>(data: T = null): Result<T> {
    return new Result<T>(data, null);
  }

  public static fail<TError extends Error = Error>(
    error: TError
  ): Result<null, TError>;
  public static fail<T, TError extends Error = Error>(
    error: TError
  ): Result<T, TError>;
  public static fail<T, TError extends Error = Error>(
    error: TError
  ): Result<T, TError> {
    return new Result<T, TError>(null, error);
  }
}
