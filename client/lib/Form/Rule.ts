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

export class EmailRule implements Rule {
  private static regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

  constructor(readonly message: string = "Must be a valid email.") {}

  validate(value: string): string | null {
    if (!EmailRule.regex.test(value)) return this.message;

    return null;
  }
}

export class PasswordRule implements Rule {
  constructor(
    readonly message: string = "Password must be at least 8 characters long."
  ) {}

  validate(value: string): string | null {
    if (value.length < 8) return this.message;

    return null;
  }
}

export class PhoneRule implements Rule {
  private static mobileRegex = /^(\+44 ?|0)7[0-9]{9}$/;
  private static landlineRegex = /^[0-9]{5} ?[0-9]{6}$/;

  constructor(readonly message: string = "Must be a valid phone number.") {}

  validate(value: string): string | null {
    if (
      !PhoneRule.mobileRegex.test(value) &&
      !PhoneRule.landlineRegex.test(value)
    )
      return this.message;

    return null;
  }
}

export class PostCodeRule implements Rule {
  private static regex = /^[A-Z]{2}[0-9]{1,2} ?[0-9][A-Z]{2}$/i;

  constructor(readonly message: string = "Must be a valid post code.") {}

  validate(value: string): string | null {
    if (!PostCodeRule.regex.test(value)) return this.message;

    return null;
  }
}
