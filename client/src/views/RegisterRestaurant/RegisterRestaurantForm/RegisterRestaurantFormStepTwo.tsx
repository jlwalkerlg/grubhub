import React from "react";
import { UseFormMethods } from "react-hook-form";
import { combineRules, PhoneRule, RequiredRule } from "~/services/forms/Rule";

interface Props {
  form: UseFormMethods<{
    restaurantName: string;
    restaurantPhoneNumber: string;
  }>;
  advanceStep(): void;
  backStep(): void;
}

const RegisterRestaurantFormStepTwo: React.FC<Props> = ({
  form,
  advanceStep,
  backStep,
}) => {
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
            validate: combineRules([new RequiredRule()]),
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
            validate: combineRules([new RequiredRule(), new PhoneRule()]),
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
          onClick={backStep}
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
