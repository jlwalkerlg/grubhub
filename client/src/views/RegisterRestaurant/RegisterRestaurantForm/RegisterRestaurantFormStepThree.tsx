import React, { useEffect } from "react";
import { useForm } from "react-hook-form";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useRules } from "~/services/useRules";
import { setFormErrors } from "~/services/utils";

interface StepThreeValues {
  addressLine1: string;
  addressLine2: string;
  city: string;
  postcode: string;
}

interface Props {
  isSubmitting: boolean;
  defaults: StepThreeValues;
  errors: { [K in keyof StepThreeValues]?: string[] };
  backStep(data: StepThreeValues): any;
  onSubmit(data: StepThreeValues): any;
}

const RegisterRestaurantFormStepThree: React.FC<Props> = ({
  isSubmitting,
  defaults,
  errors,
  backStep,
  onSubmit,
}) => {
  const form = useForm<StepThreeValues>({
    defaultValues: defaults,
  });

  const rules = useRules({
    addressLine1: (builder) => builder.required(),
    city: (builder) => builder.required(),
    postcode: (builder) => builder.required().postcode(),
  });

  useEffect(() => {
    setFormErrors(errors, form);
  }, [errors]);

  return (
    <form onSubmit={form.handleSubmit(onSubmit)}>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Address
      </p>

      <div className="mt-4">
        <label className="label" htmlFor="addressLine1">
          Address Line 1 <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.addressLine1,
          })}
          autoFocus
          className="input"
          type="text"
          name="addressLine1"
          id="addressLine1"
          data-invalid={!!form.errors.addressLine1}
        />
        {form.errors.addressLine1 && (
          <p className="form-error mt-1">{form.errors.addressLine1.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="addressLine2">
          Address Line 2
        </label>
        <input
          ref={form.register}
          className="input"
          type="text"
          name="addressLine2"
          id="addressLine2"
          data-invalid={!!form.errors.addressLine2}
        />
        {form.errors.addressLine2 && (
          <p className="form-error mt-1">{form.errors.addressLine2.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="city">
          City <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.city,
          })}
          className="input"
          type="text"
          name="city"
          id="city"
          data-invalid={!!form.errors.city}
        />
        {form.errors.city && (
          <p className="form-error mt-1">{form.errors.city.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="postcode">
          Postcode <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.postcode,
          })}
          className="input"
          type="text"
          name="postcode"
          id="postcode"
          data-invalid={!!form.errors.postcode}
        />
        {form.errors.postcode && (
          <p className="form-error mt-1">{form.errors.postcode.message}</p>
        )}
      </div>

      <div className="mt-8">
        <button
          type="button"
          className="btn btn-outline-primary font-semibold w-full"
          onClick={() => backStep(form.getValues())}
        >
          Back
        </button>
      </div>

      <div className="mt-3">
        <button
          type="submit"
          className="btn btn-primary font-semibold w-full"
          disabled={isSubmitting}
        >
          {isSubmitting ? (
            <SpinnerIcon className="h-6 w-6 inline-block animate-spin" />
          ) : (
            <span>Register</span>
          )}
        </button>
      </div>
    </form>
  );
};

export default RegisterRestaurantFormStepThree;
