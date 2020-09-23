import { FieldName, UseFormMethods } from "react-hook-form";

export const setFormErrors = <T>(
  errors: { [key: string]: string },
  form: UseFormMethods<T>
) => {
  for (const field in errors) {
    if (Object.prototype.hasOwnProperty.call(errors, field)) {
      const message = errors[field];
      form.setError(field as FieldName<T>, { message });
    }
  }
};
