import { useState, useEffect } from "react";
import { FormComponent } from "./useFormComponent";

export interface Form {
  isValid: boolean;
  values: { [key: string]: string };
}

export default function useForm(components: {
  [key: string]: FormComponent;
}): Form {
  const checkValid = () => {
    for (const field in components) {
      if (Object.prototype.hasOwnProperty.call(components, field)) {
        const component = components[field];

        if (!component.validate()) return false;
      }
    }

    return true;
  };

  const [isValid, setIsValid] = useState(checkValid);
  useEffect(() => setIsValid(checkValid()), [components]);

  const values = () => {
    const values = {};

    for (const field in components) {
      if (Object.prototype.hasOwnProperty.call(components, field)) {
        const component = components[field];
        values[field] = component.value;
      }
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
