export interface Rule {
  message: string;
  validate(value: string): string | null;
}

export const combineRules = (rules: Rule[]) => {
  return (value: any) => {
    for (const rule of rules) {
      const error = rule.validate(value);

      if (error !== null) {
        return error;
      }
    }

    return null;
  };
};

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
  private static landlineRegex = /^[0-9]{5} ?[0-9]{6}$/;

  constructor(readonly message: string = "Must be a valid phone number.") {}

  validate(value: string): string | null {
    if (!PhoneRule.landlineRegex.test(value)) return this.message;

    return null;
  }
}