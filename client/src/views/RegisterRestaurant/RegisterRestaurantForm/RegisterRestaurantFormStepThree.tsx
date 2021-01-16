import React, { useEffect, useRef } from "react";
import { useForm } from "react-hook-form";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { setFormErrors } from "~/services/forms/setFormErrors";
import useAddressLookup from "~/services/geolocation/useAddressLookup";
import useAddressPredictions from "~/services/geolocation/useAddressPredictions";
import useAutocomplete from "~/services/useAutocomplete";

interface StepThreeValues {
  address: string;
}

interface Props {
  isSubmitting: boolean;
  defaults: StepThreeValues;
  errors: { [K in keyof StepThreeValues]?: string };
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

  useEffect(() => {
    setFormErrors(errors, form);
  }, [errors]);

  const address = form.watch("address");

  const { predictions, pause } = useAddressPredictions(address);

  const autocompleteWrapperRef = useRef<HTMLDivElement>();
  const autocompleteInputRef = useRef<HTMLInputElement>();
  const autocompleteEndRef = useRef<HTMLButtonElement>();

  const { isOpen: isAutocompleteOpen, close } = useAutocomplete(
    predictions,
    autocompleteInputRef,
    autocompleteEndRef,
    autocompleteWrapperRef
  );

  const { getAddressById } = useAddressLookup();

  const onSelectAddress = async (id: string) => {
    const address = await getAddressById(id);
    pause();
    close();
    form.setValue("address", address);
    autocompleteInputRef.current.focus();
  };

  return (
    <form onSubmit={form.handleSubmit(onSubmit)}>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Address
      </p>

      <div className="mt-4">
        <label className="label" htmlFor="address">
          Address <span className="text-primary">*</span>
        </label>

        <div ref={autocompleteWrapperRef} className="relative">
          <input
            ref={(e) => {
              autocompleteInputRef.current = e;
              form.register(e);
            }}
            autoFocus
            className="input"
            type="text"
            name="address"
            id="address"
            placeholder="e.g. 123 High Street"
            autoComplete="new-password"
            data-invalid={!!form.errors.address}
          />

          {isAutocompleteOpen && (
            <ul className="absolute top-100 w-full rounded-lg shadow">
              {predictions.map((x, index) => {
                return (
                  <li key={x.description} className="w-full">
                    <button
                      ref={
                        index === predictions.length - 1
                          ? autocompleteEndRef
                          : undefined
                      }
                      onClick={() => onSelectAddress(x.id)}
                      type="button"
                      className="py-2 px-4 w-full text-left bg-white hover:bg-gray-100 focus:bg-gray-100 border-t border-gray-300 cursor-pointer"
                    >
                      {x.description}
                    </button>
                  </li>
                );
              })}
            </ul>
          )}
        </div>

        {form.errors.address && (
          <p className="form-error mt-1">{form.errors.address.message}</p>
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
