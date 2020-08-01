export interface Rule {
  message: string;
  validate(value: string): string | null;
}

export class RequiredRule implements Rule {
  constructor(readonly message: string = "Required.") {}

  validate(value: string) {
    if (value === "") return this.message;

    return null;
  }
}

export class MinLengthRule implements Rule {
  constructor(
    private length: number,
    private getMessage: (length: number) => string = (length) =>
      `Must be at least ${length} characters long.`
  ) {}

  get message() {
    return this.getMessage(this.length);
  }

  validate(value: string): string | null {
    if (value.length < this.length) return this.message;

    return null;
  }
}
