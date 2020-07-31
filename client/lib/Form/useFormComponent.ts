import React, { FormEvent, useState } from "react";

export interface FormComponent {
  value: string;
  props: {
    value: string;
    onChange: (e: FormEvent<HTMLInputElement>) => void;
  };
}

export function useFormComponent(initialValue: string): FormComponent {
  const [value, setValue] = useState("");

  function onChange(e: FormEvent<HTMLInputElement>): void {
    setValue(e.currentTarget.value);
  }

  return {
    value,
    props: {
      value,
      onChange,
    },
  };
}
