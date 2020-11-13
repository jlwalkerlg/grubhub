import React from "react";
import { UseFormMethods } from "react-hook-form";
import { ErrorAlert } from "~/components/Alert/Alert";
import Autocomplete from "~/components/Autocomplete/Autocomplete";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import {
  combineRules,
  EmailRule,
  PasswordRule,
  PhoneRule,
  RequiredRule,
} from "~/services/forms/Rule";
import { AddressSearchResult } from "~/services/geolocation/AddressSearcher";

export interface StepOne {
  managerName: string;
  managerEmail: string;
  managerPassword: string;
}

export interface StepTwo {
  restaurantName: string;
  restaurantPhoneNumber: string;
}

export interface StepThree {
  address: string;
}

export interface Props {
  step: number;
  step1: UseFormMethods<StepOne>;
  step2: UseFormMethods<StepTwo>;
  step3: UseFormMethods<StepThree>;
  isError: boolean;
  error: string;
  addressSearchResults: AddressSearchResult[];
  onSelectAddress(id: string): void;
  advanceStep(): void;
  backStep(): void;
  onSubmit(): void;
}

const FirstStep: React.FC<Props> = ({ step1: form, advanceStep }) => {
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

const SecondStep: React.FC<Props> = ({
  step2: form,
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

const LastStep: React.FC<Props> = ({
  addressSearchResults,
  onSelectAddress,
  backStep,
  onSubmit,
  step3: form,
}) => {
  return (
    <form onSubmit={form.handleSubmit(onSubmit)}>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Address
      </p>

      <div className="mt-4">
        <label className="label" htmlFor="address">
          Address <span className="text-primary">*</span>
        </label>
        <Autocomplete
          inputRef={form.register({
            validate: combineRules([new RequiredRule()]),
          })}
          predictions={addressSearchResults}
          onSelection={onSelectAddress}
          autoFocus
          className="input"
          type="text"
          name="address"
          id="address"
          placeholder="e.g. 123 High Street"
          autoComplete="new-password"
          data-invalid={!!form.errors.address}
        ></Autocomplete>
        {form.errors.address && (
          <p className="form-error mt-1">{form.errors.address.message}</p>
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
        <button
          type="submit"
          className="btn btn-primary font-semibold w-full"
          disabled={form.formState.isSubmitting}
        >
          {form.formState.isSubmitting ? (
            <SpinnerIcon className="h-6 w-6 inline-block animate-spin" />
          ) : (
            <span>Register</span>
          )}
        </button>
      </div>
    </form>
  );
};

const RegisterRestaurantForm: React.FC<Props> = (props: Props) => {
  const { step, isError, error } = props;

  return (
    <div>
      {isError && (
        <div className="my-6">
          <ErrorAlert message={error} />
        </div>
      )}

      <div className={step !== 1 ? "sr-only" : undefined}>
        <FirstStep {...props} />
      </div>
      <div className={step !== 2 ? "sr-only" : undefined}>
        <SecondStep {...props} />
      </div>
      <div className={step !== 3 ? "sr-only" : undefined}>
        <LastStep {...props} />
      </div>
    </div>
  );
};

export default RegisterRestaurantForm;
