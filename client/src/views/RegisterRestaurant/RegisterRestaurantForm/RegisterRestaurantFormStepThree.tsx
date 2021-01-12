import React, { useEffect } from "react";
import { UseFormMethods } from "react-hook-form";
import Autocomplete from "~/components/Autocomplete/Autocomplete";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { combineRules, RequiredRule } from "~/services/forms/Rule";
import useAddressSearch from "~/services/geolocation/useAddressSearch";

interface Props {
  form: UseFormMethods<{
    address: string;
  }>;
  backStep(): void;
  onSubmit(): void;
}

const RegisterRestaurantFormStepThree: React.FC<Props> = ({
  form,
  backStep,
  onSubmit,
}) => {
  const {
    results: addressSearchResults,
    address,
    onSelectAddress,
  } = useAddressSearch(form.watch("address"));

  useEffect(() => {
    if (address !== null) {
      form.setValue("address", address);
    }
  }, [address]);

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

export default RegisterRestaurantFormStepThree;
