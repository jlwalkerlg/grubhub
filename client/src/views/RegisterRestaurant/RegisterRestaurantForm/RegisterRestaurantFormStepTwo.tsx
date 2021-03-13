import React, { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useRules } from "~/services/useRules";
import { setFormErrors } from "~/services/utils";

interface StepTwoValues {
  restaurantName: string;
  restaurantPhoneNumber: string;
}

interface Props {
  defaults: StepTwoValues;
  errors: { [K in keyof StepTwoValues]?: string[] };
  advanceStep(data: StepTwoValues): any;
  backStep(data: StepTwoValues): any;
}

const RegisterRestaurantFormStepTwo: React.FC<Props> = ({
  defaults,
  errors,
  advanceStep,
  backStep,
}) => {
  const form = useForm<StepTwoValues>({
    defaultValues: defaults,
  });

  const rules = useRules(() => ({
    restaurantName: (builder) => builder.required(),
    restaurantPhoneNumber: (builder) => builder.required().phone(),
  }));

  useEffect(() => {
    setFormErrors(errors, form);
  }, [errors]);

  return (
    <form onSubmit={form.handleSubmit(advanceStep)}>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Details
      </p>

      <div className="mt-6">
        <label className="label" htmlFor="restaurantName">
          Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.restaurantName,
          })}
          autoFocus
          className="input"
          type="text"
          name="restaurantName"
          id="restaurantName"
          data-invalid={!!form.errors.restaurantName}
        />
        {form.errors.restaurantName && (
          <p className="form-error mt-1">
            {form.errors.restaurantName.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="restaurantPhoneNumber">
          Phone Number <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.restaurantPhoneNumber,
          })}
          className="input"
          type="tel"
          name="restaurantPhoneNumber"
          id="restaurantPhoneNumber"
          placeholder="e.g. 01234 567890"
          data-invalid={!!form.errors.restaurantPhoneNumber}
        />
        {form.errors.restaurantPhoneNumber && (
          <p className="form-error mt-1">
            {form.errors.restaurantPhoneNumber.message}
          </p>
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
        <button type="submit" className="btn btn-primary font-semibold w-full">
          Continue
        </button>
      </div>
    </form>
  );
};

export default RegisterRestaurantFormStepTwo;
