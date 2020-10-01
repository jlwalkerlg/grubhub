import React from "react";

import { AddressSearchResult } from "~/services/geolocation/AddressSearcher";
import Autocomplete from "~/components/Autocomplete/Autocomplete";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { UseFormMethods } from "react-hook-form";
import { ErrorAlert } from "~/components/Alert/Alert";

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
  addressLine1: string;
  addressLine2: string;
  town: string;
  postcode: string;
}

export interface Props {
  step: number;
  step1: UseFormMethods<StepOne>;
  step2: UseFormMethods<StepTwo>;
  step3: UseFormMethods<StepThree>;
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
          ref={form.register}
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
          ref={form.register}
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
          ref={form.register}
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
        <button
          type="submit"
          className="btn btn-primary font-semibold w-full"
          disabled={!form.formState.isValid}
        >
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
          ref={form.register}
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
          ref={form.register}
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
        <button
          type="submit"
          className="btn btn-primary font-semibold w-full"
          disabled={!form.formState.isValid}
        >
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
        <label className="label" htmlFor="addressLine1">
          Address Line 1 <span className="text-primary">*</span>
        </label>
        <Autocomplete
          inputRef={form.register}
          predictions={addressSearchResults}
          onSelection={onSelectAddress}
          autoFocus
          className="input"
          type="text"
          name="addressLine1"
          id="addressLine1"
          placeholder="e.g. 123 High Street"
          autoComplete="new-password"
          data-invalid={!!form.errors.addressLine1}
        ></Autocomplete>
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
        <label className="label" htmlFor="town">
          Town / City <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register}
          className="input"
          type="text"
          name="town"
          id="town"
          placeholder="e.g. Manchester"
          data-invalid={!!form.errors.town}
        />
        {form.errors.town && (
          <p className="form-error mt-1">{form.errors.town.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="postcode">
          Post Code <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register}
          className="input"
          type="text"
          name="postcode"
          id="postcode"
          placeholder="e.g. AB12 3CD"
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
          onClick={backStep}
        >
          Back
        </button>
      </div>

      <div className="mt-3">
        <button
          type="submit"
          className="btn btn-primary font-semibold w-full"
          disabled={!form.formState.isValid}
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
  const { step, error } = props;

  return (
    <div>
      {error && (
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
