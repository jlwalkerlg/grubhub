export class Error {
  public constructor(message: string);
  public constructor(
    message: string,
    statusCode: number,
    errors: { [key: string]: string }
  );
  public constructor(
    readonly message: string,
    readonly statusCode: number = null,
    readonly errors: { [key: string]: string } = null
  ) {}

  get isValidationError() {
    return this.errors !== null;
  }
}
