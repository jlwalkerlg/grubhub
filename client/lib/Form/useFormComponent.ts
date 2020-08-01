import React, { FormEvent, useState } from "react";
import { Rule } from "./Rule";

export interface FormComponent {
  value: string;
  dirty: boolean;
  error: string;
  validate(): boolean;
  props: {
    value: string;
    valid: "true" | "false" | undefined;
    onChange: (e: FormEvent<HTMLInputElement>) => void;
  };
}

export interface UseFormComponentOptions {}

export function useFormComponent(
  initialValue: string,
  rules: Rule[] = [],
  options: UseFormComponentOptions = {}
): FormComponent {
  const [value, setValue] = useState(initialValue);
  const [dirty, setDirty] = useState(false);
  const [error, setError] = useState<string>(null);
  const [valid, setValid] = useState(undefined);

  function onChange(e: FormEvent<HTMLInputElement>): void {
    setDirty(true);
    setValue(e.currentTarget.value);
  }

  function validate(): boolean {
    for (const rule of rules) {
      const error = rule.validate(value);
      setError(error);

      if (error !== null) {
        setValid("false");
        return false;
      }
    }

    setValid("true");
    return true;
  }

  return {
    value,
    dirty,
    error,
    validate,
    props: {
      value,
      valid,
      onChange,
    },
  };
}
