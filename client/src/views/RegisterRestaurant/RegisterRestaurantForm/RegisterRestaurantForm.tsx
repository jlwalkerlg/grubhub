import React, {
  FC,
  FormEvent,
  SyntheticEvent,
  KeyboardEvent,
  MutableRefObject,
} from "react";

import { FormComponent } from "~/lib/Form/useFormComponent";
import { AddressSearchResult } from "~/lib/AddressSearch/AddressSearcher";
import FormError from "~/components/FormError/FormError";
import Autocomplete from "~/components/Autocomplete/Autocomplete";

export interface Props {
  addressSearchResults: AddressSearchResult[];
  onSelectAddress(id: string): void;
  clearAddressSearchResults(): void;
  onKeydownAddressLine1(e: KeyboardEvent): void;
  addressLine1Ref: MutableRefObject<HTMLInputElement>;
  managerName: FormComponent;
  managerEmail: FormComponent;
  managerPassword: FormComponent;
  restaurantName: FormComponent;
  restaurantPhone: FormComponent;
  addressLine1: FormComponent;
  addressLine2: FormComponent;
  city: FormComponent;
  postCode: FormComponent;
  step: number;
  canAdvance: boolean;
  advanceStep(e: SyntheticEvent): void;
  backStep(e: SyntheticEvent): void;
  onSubmit(e: FormEvent): void;
}

const FirstStep: FC<Props> = ({
  managerName,
  managerEmail,
  managerPassword,
  canAdvance,
  advanceStep,
}) => {
  return (
    <div>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Manager Details
      </p>

      <div className="mt-6">
        <label className="label" htmlFor="managerName">
          Manager Name <span className="text-primary">*</span>
        </label>
        <input
          {...managerName.props}
          className="input"
          type="text"
          name="managerName"
          id="managerName"
        />
        <FormError component={managerName} className="mt-1" />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="managerEmail">
          Manager Email <span className="text-primary">*</span>
        </label>
        <input
          {...managerEmail.props}
          className="input"
          type="email"
          name="managerEmail"
          id="managerEmail"
          placeholder="e.g. email@email.com"
        />
        <FormError component={managerEmail} className="mt-1" />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="managerPassword">
          Manager Password <span className="text-primary">*</span>
        </label>
        <input
          {...managerPassword.props}
          className="input"
          type="password"
          name="managerPassword"
          id="managerPassword"
        />
        <FormError component={managerPassword} className="mt-1" />
      </div>

      <div className="mt-8">
        <button
          className="btn btn-primary font-semibold w-full"
          onClick={advanceStep}
          role="button"
          disabled={!canAdvance}
        >
          Continue
        </button>
      </div>
    </div>
  );
};

const SecondStep: FC<Props> = ({
  restaurantName,
  restaurantPhone,
  canAdvance,
  advanceStep,
  backStep,
}) => {
  return (
    <div>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Details
      </p>

      <div className="mt-6">
        <label className="label" htmlFor="restaurantName">
          Name <span className="text-primary">*</span>
        </label>
        <input
          {...restaurantName.props}
          className="input"
          type="text"
          name="restaurantName"
          id="restaurantName"
        />
        <FormError component={restaurantName} className="mt-1" />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="restaurantPhone">
          Phone Number <span className="text-primary">*</span>
        </label>
        <input
          {...restaurantPhone.props}
          className="input"
          type="tel"
          name="restaurantPhone"
          id="restaurantPhone"
          placeholder="e.g. 01234 567890"
        />
        <FormError component={restaurantPhone} className="mt-1" />
      </div>

      <div className="mt-8">
        <button
          className="btn btn-outline-primary font-semibold w-full"
          onClick={backStep}
        >
          Back
        </button>
      </div>

      <div className="mt-3">
        <button
          className="btn btn-primary font-semibold w-full"
          onClick={advanceStep}
          disabled={!canAdvance}
        >
          Continue
        </button>
      </div>
    </div>
  );
};

const LastStep: FC<Props> = ({
  addressSearchResults,
  onSelectAddress,
  clearAddressSearchResults,
  onKeydownAddressLine1,
  addressLine1Ref,
  addressLine1,
  addressLine2,
  city,
  postCode,
  canAdvance,
  backStep,
  onSubmit,
}) => {
  return (
    <div>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Address
      </p>

      <div className="mt-4">
        <label className="label" htmlFor="addressLine1">
          Address Line 1 <span className="text-primary">*</span>
        </label>
        <div className="relative">
          <input
            {...addressLine1.props}
            onKeyDown={onKeydownAddressLine1}
            ref={addressLine1Ref}
            className="input"
            type="text"
            name="addressLine1"
            id="addressLine1"
            placeholder="e.g. 123 High Street"
          />
          <Autocomplete
            predictions={addressSearchResults}
            onSelect={onSelectAddress}
            clear={clearAddressSearchResults}
          />
        </div>
        <FormError component={addressLine1} className="mt-1" />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="addressLine2">
          Address Line 2
        </label>
        <input
          {...addressLine2.props}
          className="input"
          type="text"
          name="addressLine2"
          id="addressLine2"
        />
        <FormError component={addressLine2} className="mt-1" />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="city">
          Town / City <span className="text-primary">*</span>
        </label>
        <input
          {...city.props}
          className="input"
          type="text"
          name="city"
          id="city"
          placeholder="e.g. Manchester"
        />
        <FormError component={city} className="mt-1" />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="postCode">
          Post Code <span className="text-primary">*</span>
        </label>
        <input
          {...postCode.props}
          className="input"
          type="text"
          name="postCode"
          id="postCode"
          placeholder="e.g. AB12 3CD"
        />
        <FormError component={postCode} className="mt-1" />
      </div>

      <div className="mt-8">
        <button
          className="btn btn-outline-primary font-semibold w-full"
          onClick={backStep}
        >
          Back
        </button>
      </div>

      <div className="mt-3">
        <button
          className="btn btn-primary font-semibold w-full"
          onClick={onSubmit}
          disabled={!canAdvance}
        >
          Register
        </button>
      </div>
    </div>
  );
};

const RegisterRestaurantForm: FC<Props> = (props: Props) => {
  const { step, onSubmit } = props;

  return (
    <form action="/restaurants/register" method="POST" onSubmit={onSubmit}>
      {step === 1 && <FirstStep {...props} />}
      {step === 2 && <SecondStep {...props} />}
      {step === 3 && <LastStep {...props} />}
    </form>
  );
};

export default RegisterRestaurantForm;
