import React, { useEffect } from "react";
import { useForm } from "react-hook-form";
import { setFormErrors } from "~/services/forms/setFormErrors";
import { useRules } from "~/services/forms/useRules";

interface StepOneValues {
  managerFirstName: string;
  managerLastName: string;
  managerEmail: string;
  managerPassword: string;
}

interface Props {
  defaults: StepOneValues;
  errors: { [K in keyof StepOneValues]?: string[] };
  advanceStep(data: StepOneValues): any;
}

const RegisterRestaurantFormStepOne: React.FC<Props> = ({
  defaults,
  errors,
  advanceStep,
}) => {
  const form = useForm<StepOneValues>({
    defaultValues: defaults,
  });

  const rules = useRules(() => ({
    managerFirstName: (builder) => builder.required(),
    managerLastName: (builder) => builder.required(),
    managerEmail: (builder) => builder.required().email(),
    managerPassword: (builder) => builder.required().password(),
  }));

  useEffect(() => {
    setFormErrors(errors, form);
  }, [errors]);

  return (
    <form onSubmit={form.handleSubmit(advanceStep)}>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Manager Details
      </p>

      <div className="mt-6">
        <label className="label" htmlFor="managerFirstName">
          First Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.managerFirstName,
          })}
          autoFocus
          className="input"
          type="text"
          name="managerFirstName"
          id="managerFirstName"
          data-invalid={!!form.errors.managerFirstName}
        />
        {form.errors.managerFirstName && (
          <p className="form-error mt-1">
            {form.errors.managerFirstName.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="managerLastName">
          Last Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.managerLastName,
          })}
          className="input"
          type="text"
          name="managerLastName"
          id="managerLastName"
          data-invalid={!!form.errors.managerLastName}
        />
        {form.errors.managerLastName && (
          <p className="form-error mt-1">
            {form.errors.managerLastName.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="managerEmail">
          Email <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.managerEmail,
          })}
          className="input"
          type="email"
          name="managerEmail"
          id="managerEmail"
          placeholder="e.g. email@email.com"
          data-invalid={!!form.errors.managerEmail}
        />
        {form.errors.managerEmail && (
          <p className="form-error mt-1">{form.errors.managerEmail.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="managerPassword">
          Password <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.managerPassword,
          })}
          className="input"
          type="password"
          name="managerPassword"
          id="managerPassword"
        />
        {form.errors.managerPassword && (
          <p className="form-error mt-1">
            {form.errors.managerPassword.message}
          </p>
        )}
      </div>

      <div className="mt-8">
        <button type="submit" className="btn btn-primary font-semibold w-full">
          Continue
        </button>
      </div>
    </form>
  );
};

export default RegisterRestaurantFormStepOne;
