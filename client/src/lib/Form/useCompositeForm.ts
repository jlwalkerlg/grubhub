import { useState, useEffect } from "react";
import { Form } from "./useForm";

interface CompositeForm {
  isValid: boolean;
  values: { [key: string]: string };
}

export default function useCompositeForm(forms: Form[]): CompositeForm {
  const checkValid = () => {
    return !forms.some((x) => !x.isValid);
  };

  const [isValid, setIsValid] = useState(checkValid);
  useEffect(() => setIsValid(checkValid()), [forms]);

  const values = () => {
    let values = {};

    for (const form of forms) {
      values = { ...values, ...form.values };
    }

    return values;
  };

  return {
    isValid,
    get values() {
      return values();
    },
  };
}
