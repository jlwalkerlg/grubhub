import { useState, useEffect } from "react";
import { FormComponent } from "./useFormComponent";

interface CompositeForm {
  isValid: boolean;
  isStepValid: boolean;
  values: { [key: string]: string };
}

export default function useCompositeForm(
  forms: Array<{ [key: string]: FormComponent }>,
  step: number
): CompositeForm {
  const checkValid = () => {
    for (const form of forms) {
      for (const field in form) {
        if (Object.prototype.hasOwnProperty.call(form, field)) {
          const component = form[field];
          if (!component.valid) return false;
        }
      }
    }

    return true;
  };

  const checkStepValid = () => {
    const form = forms[step - 1];

    for (const field in form) {
      if (Object.prototype.hasOwnProperty.call(form, field)) {
        const component = form[field];
        if (!component.valid) return false;
      }
    }
    return true;
  };

  const [isValid, setIsValid] = useState(checkValid);
  useEffect(() => setIsValid(checkValid()), [forms]);

  const [isStepValid, setIsStepValid] = useState(checkStepValid);
  useEffect(() => setIsStepValid(checkStepValid()), [forms[step - 1]]);

  const values = () => {
    let values = {};

    for (const form of forms) {
      values = { ...values, ...form.values };
    }

    return values;
  };

  return {
    isValid,
    isStepValid,
    get values() {
      return values();
    },
  };
}
