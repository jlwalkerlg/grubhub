import { useMemo } from "react";

type ValidateFunction = (value?: string) => true | string;

class RuleBuilder {
  private static emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-z\-0-9]+\.)+[a-z]{2,}))$/i;
  private static postcodeRegex = /^([a-z]{1,2}[0-9]{1,2}|[a-z]{1,2}[0-9][a-z]) ?[0-9]{1}[a-z]{2}$/i;
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

      value = value.replaceAll("+44", "0").replaceAll(" ", "");

      const number = +value;

      if (!Number.isInteger(number)) return false;

      return number >= 7000000000 && number < 8000000000;
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

type Rules = Record<string, (builder: RuleBuilder) => RuleBuilder>;

export function useRules<T extends Rules>(schema: T) {
  return useMemo(() => {
    return Object.fromEntries(
      Object.entries(schema).map(([key, builder]) => {
        return [key, builder(new RuleBuilder()).build()];
      })
    ) as Record<keyof T, ValidateFunction>;
  }, []);
}
