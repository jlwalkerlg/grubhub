import React from "react";
import { UseFormMethods } from "react-hook-form";
import {
  combineRules,
  EmailRule,
  PasswordRule,
  RequiredRule,
} from "~/services/forms/Rule";

interface Props {
  form: UseFormMethods<{
    managerName: string;
    managerEmail: string;
    managerPassword: string;
  }>;
  advanceStep(): void;
}

const RegisterRestaurantFormStepOne: React.FC<Props> = ({
  form,
  advanceStep,
}) => {
  return (
    <form onSubmit={form.handleSubmit(advanceStep)}>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Manager Details
      </p>

      <div className="mt-6">
        <label className="label" htmlFor="managerName">
          Manager Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: combineRules([new RequiredRule()]),
          })}
          autoFocus
          className="input"
          type="text"
          name="managerName"
          id="managerName"
          data-invalid={!!form.errors.managerName}
        />
        {form.errors.managerName && (
          <p className="form-error mt-1">{form.errors.managerName.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="managerEmail">
          Manager Email <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: combineRules([new RequiredRule(), new EmailRule()]),
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
          Manager Password <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: combineRules([new RequiredRule(), new PasswordRule()]),
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
