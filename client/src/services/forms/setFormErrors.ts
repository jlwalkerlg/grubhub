import { FieldName, UseFormMethods } from "react-hook-form";

export const setFormErrors = <T>(
  errors: { [key: string]: string[] },
  form: UseFormMethods<T>
) => {
  for (const field in errors) {
    if (form.getValues().hasOwnProperty(field)) {
      const message = errors[field][0] || undefined;
      form.setError(field as FieldName<T>, { message });
    }
  }
};
