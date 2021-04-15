import { useState } from "react";
import {
  FieldName,
  FieldValues,
  SubmitErrorHandler,
  SubmitHandler,
  useForm as useHookForm,
  UseFormOptions
} from "react-hook-form";
import api from "~/api/api";

export default function useForm<
  TFieldValues extends FieldValues = FieldValues,
  TContext extends object = object
>(options?: UseFormOptions<TFieldValues, TContext>) {
  const form = useHookForm(options);

  const [error, setError] = useState<Error>();
  const [hasValidationErrors, setHasValidationErrors] = useState(false);

  const handleSubmit = <TSubmitFieldValues extends FieldValues = TFieldValues>(
    onValid: SubmitHandler<TSubmitFieldValues>,
    onInvalid?: SubmitErrorHandler<TFieldValues>
  ) => {
    return form.handleSubmit<TSubmitFieldValues>(async (values, event) => {
      if (form.formState.isSubmitting) return;

      try {
        await onValid(values, event);
        setError(undefined);
        setHasValidationErrors(false);
      } catch (e) {
        if (api.isApiError(e) && e.status === 422) {
          for (const field in e.errors) {
            if (Object.prototype.hasOwnProperty.call(e.errors, field)) {
              const message = e.errors[field][0];
              form.setError(field as FieldName<TFieldValues>, { message });
            }
          }

          setHasValidationErrors(true);
        }

        setError(e);
      }
    }, onInvalid);
  };

  const setErrors = (errors: { [key: string]: string[] }) => {
    const values = form.getValues();

    for (const field in errors) {
      if (values.hasOwnProperty(field)) {
        const message = errors[field][0] || undefined;
        form.setError(field as FieldName<TFieldValues>, { message });
      }
    }
  };

  const isSuccess = form.formState.isSubmitSuccessful && !error;
  const isLoading = form.formState.isSubmitting;

  return {
    ...form,
    handleSubmit,
    setErrors,
    error,
    isSuccess,
    isLoading,
    hasValidationErrors,
  };
}
