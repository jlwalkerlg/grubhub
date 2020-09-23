import { Error } from "./Error";

export class Result<T> {
  readonly data?: T;
  readonly error?: Error;

  get isSuccess() {
    return this.error === null;
  }

  private constructor(data: T, error: Error) {
    this.data = data;
    this.error = error;
  }

  public static ok<T = null>(data: T = null): Result<T> {
    return new Result<T>(data, null);
  }

  public static fail<T = null>(error: Error): Result<T> {
    return new Result<T>(null, error);
  }
}
