import { useMemo } from "react";

type ValidateFunction = (value?: string) => true | string;

class RuleBuilder {
  private static emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  private static postcodeRegex = /^[A-Za-z]{2}\d{1,2} ?\d[A-Za-z]{2}$/;
  private static landlineRegex = /^[0-9]{5} ?[0-9]{6}$/;

  private fns: ValidateFunction[] = [];
  private isRequired = false;

  required(message: string = "Required.") {
    this.isRequired = true;
    this.fns.push((value) => (value?.trim() ? true : message));
    return this;
  }

  email(message: string = "Must be a valid email.") {
    return this.must((value) => RuleBuilder.emailRegex.test(value), message);
  }

  postcode(message: string = "Must be a valid postcode.") {
    return this.must((value) => RuleBuilder.postcodeRegex.test(value), message);
  }

  password(message: string = "Password must be at last 8 characters long.") {
    return this.minLength(8, message);
  }

  phone(message: string = "Must be a valid phone number.") {
    return this.must((value) => RuleBuilder.landlineRegex.test(value), message);
  }

  mobile(message: string = "Must be a valid mobile number.") {
    return this.must((value) => {
      if (!value) return false;

      const numbers = value
        .split("")
        .filter((x) => (+x).toString() === x && +x >= 0 && +x <= 9)
        .join("");

      if (numbers.startsWith("07") && numbers.length === 11) {
        return true;
      }

      if (numbers.startsWith("447") && numbers.length === 12) {
        return true;
      }

      return false;
    }, message);
  }

  integer(message: string = "Must be an integer.") {
    return this.must((value) => Number.isInteger(+value), message);
  }

  min(
    min: number,
    message: string = `Must be greater than or equal to ${min}.`
  ) {
    return this.must((value) => +value >= min, message);
  }

  minLength(
    minLength: number,
    message: string = `Must be at least ${minLength} characters in length.`
  ) {
    this.must((value) => value?.trim().length >= minLength, message);
    return this;
  }

  maxLength(
    maxLength: number,
    message: string = `Must not be longer than ${maxLength} characters in length.`
  ) {
    return this.must((value) => value?.trim().length <= maxLength, message);
  }

  match(toMatch: () => string, message: string) {
    return this.must((value) => value === toMatch(), message);
  }

  must(validate: (value: string) => boolean, message: string) {
    this.fns.push((value) => {
      if (validate(value)) return true;
      if (!this.isRequired && !value) return true;
      return message;
    });

    return this;
  }

  build() {
    return (value: string) => {
      for (const fn of this.fns) {
        const result = fn(value);
        if (result !== true) {
          return result;
        }
      }

      return true;
    };
  }
}

export function useRules<
  T extends Record<string, (builder: RuleBuilder) => RuleBuilder>
>(config: () => T) {
  return useMemo(() => {
    const schema = config();

    const rules: { [K in keyof T]?: ValidateFunction } = {};

    for (const key in schema) {
      if (Object.prototype.hasOwnProperty.call(schema, key)) {
        const builder = new RuleBuilder();
        rules[key] = schema[key](builder).build();
      }
    }

    return rules as { [K in keyof T]: ValidateFunction };
  }, []);
}
