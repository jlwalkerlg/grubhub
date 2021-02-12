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

export class PostcodeRule implements Rule {
  private static regex = /^[A-Za-z]{2}\d{1,2} ?\d[A-Za-z]{2}$/;

  constructor(readonly message: string = "Must be a valid postcode.") {}

  validate(value: string): string | null {
    if (!PostcodeRule.regex.test(value)) return this.message;

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

export class MobileRule implements Rule {
  constructor(readonly message: string = "Must be a valid phone number.") {}

  validate(value: string): string | null {
    if (!value) return this.message;

    const numbers = value
      .split("")
      .filter((x) => (+x).toString() === x && +x >= 0 && +x <= 9)
      .join("");

    if (numbers.startsWith("07") && numbers.length === 11) {
      return null;
    }

    if (numbers.startsWith("447") && numbers.length === 12) {
      return null;
    }

    return this.message;
  }
}

export class MinRule implements Rule {
  private min: number;
  readonly message: string;

  constructor(min: number);
  constructor(min: number, message: string);
  constructor(min: number, message: string = null) {
    this.min = min;

    if (message === null) {
      this.message = `Minimum value is ${min}.`;
    }
  }

  validate(value: string): string | null {
    if (+value < this.min) return this.message;

    return null;
  }
}

export class MaxLengthRule implements Rule {
  private max: number;
  readonly message: string;

  constructor(max: number);
  constructor(max: number, message: string);
  constructor(max: number, message: string = null) {
    this.max = max;

    if (message === null) {
      this.message = `Must not be longer than ${max} characters.`;
    }
  }

  validate(value: string): string | null {
    if (value?.length > this.max) return this.message;

    return null;
  }
}
