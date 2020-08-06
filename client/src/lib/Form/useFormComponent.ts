import { useState, SyntheticEvent, useRef } from "react";
import { Rule, RequiredRule } from "./Rule";

export interface FormComponent {
  value: string;
  touched: boolean;
  dirty: boolean;
  valid: boolean;
  error: string;
  validate(): boolean;
  setValue(val: string): void;
  props: {
    value: string;
    valid: "true" | "false" | undefined;
    touched: "true" | "false" | undefined;
    dirty: "true" | "false" | undefined;
    onChange(e: SyntheticEvent<HTMLInputElement>): void;
    onBlur(e: SyntheticEvent): void;
  };
}

export function useFormComponent(
  initialValue: string,
  rules: Rule[] = []
): FormComponent {
  const required = useRef(rules.some((x) => x instanceof RequiredRule)).current;

  const [value, setValue] = useState(initialValue);
  const [touched, setTouched] = useState(false);
  const [dirty, setDirty] = useState(false);
  const [error, setError] = useState<string>(null);
  const [valid, setValid] = useState(() => isValid(value));

  function isValid(value: string): boolean {
    if (!required && value === "") return true;

    for (const rule of rules) {
      const error = rule.validate(value);

      if (error !== null) {
        return false;
      }
    }

    return true;
  }

  function validate(v: string = value): boolean {
    if (!required && v === "") return true;

    for (const rule of rules) {
      const error = rule.validate(v);

      if (error !== null) {
        setError(error);
        setValid(false);
        return false;
      }
    }

    setError(null);
    setValid(true);
    return true;
  }

  function onBlur(e: SyntheticEvent<HTMLInputElement>): void {
    setTouched(true);
    validate(e.currentTarget.value);
  }

  function update(val: string) {
    setDirty(true);
    setValue(val);
    validate(val);
  }

  function onChange(e: SyntheticEvent<HTMLInputElement>): void {
    update(e.currentTarget.value);
  }

  return {
    value,
    touched,
    dirty,
    valid,
    error,
    validate,
    setValue: update,
    props: {
      value,
      valid: valid ? "true" : "false",
      touched: touched ? "true" : "false",
      dirty: dirty ? "true" : "false",
      onChange,
      onBlur,
    },
  };
}
