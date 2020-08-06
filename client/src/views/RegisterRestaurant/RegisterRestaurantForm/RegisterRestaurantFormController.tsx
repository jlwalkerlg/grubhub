import React, {
  FC,
  FormEvent,
  useState,
  SyntheticEvent,
  useEffect,
  KeyboardEvent,
  useRef,
} from "react";

import {
  RequiredRule,
  PasswordRule,
  EmailRule,
  PhoneRule,
  PostCodeRule,
} from "~/lib/Form/Rule";
import { useFormComponent } from "~/lib/Form/useFormComponent";
import useCompositeForm from "~/lib/Form/useCompositeForm";
import useAddressSearch from "~/lib/AddressSearch/useAddressSearch";
import useClickAwayListener from "~/lib/ClickAwayListener/useClickAwayListener";

import RegisterRestaurantForm from "./RegisterRestaurantForm";

const RegisterRestaurantFormController: FC = () => {
  const managerName = useFormComponent("Jordan Walker", [new RequiredRule()]);
  const managerEmail = useFormComponent("jordan@walker.com", [
    new RequiredRule(),
    new EmailRule(),
  ]);
  const managerPassword = useFormComponent("password123", [
    new RequiredRule(),
    new PasswordRule(),
  ]);
  const restaurantName = useFormComponent("Chow Main", [new RequiredRule()]);
  const restaurantPhone = useFormComponent("01274 788944", [
    new RequiredRule(),
    new PhoneRule(),
  ]);
  const addressLine1 = useFormComponent("", [new RequiredRule()]);
  const addressLine2 = useFormComponent("");
  const city = useFormComponent("", [new RequiredRule()]);
  const postCode = useFormComponent("", [
    new RequiredRule(),
    new PostCodeRule(),
  ]);

  const {
    results: addressSearchResults,
    address,
    onSelectAddress,
    clear: clearAddressSearchResults,
  } = useAddressSearch(addressLine1.value);

  useEffect(() => {
    if (address !== null) {
      addressLine1.setValue(address.addressLine1);
      addressLine2.setValue(address.addressLine2);
      city.setValue(address.city);
      postCode.setValue(address.postCode);
    }
  }, [address]);

  const [step, setStep] = useState(1);

  const form = useCompositeForm(
    [
      { managerName, managerEmail, managerPassword },
      { restaurantName, restaurantPhone },
      { addressLine1, addressLine2, city, postCode },
    ],
    step
  );

  function advanceStep(e: SyntheticEvent) {
    e.preventDefault();

    if (form.isStepValid) {
      setStep(step + 1);
    }
  }

  function backStep(e: SyntheticEvent) {
    e.preventDefault();

    setStep(step - 1);
  }

  function onSubmit(e: FormEvent) {
    e.preventDefault();

    console.log("values", form.values);
  }

  const onKeydownAddressLine1 = (e: KeyboardEvent) => {
    if (e.key === "Tab" && e.shiftKey) {
      clearAddressSearchResults();
    }
  };

  const addressLine1Ref = useRef<HTMLInputElement>(null);
  useClickAwayListener(addressLine1Ref, clearAddressSearchResults);

  return (
    <RegisterRestaurantForm
      addressSearchResults={addressSearchResults}
      onSelectAddress={onSelectAddress}
      clearAddressSearchResults={clearAddressSearchResults}
      onKeydownAddressLine1={onKeydownAddressLine1}
      addressLine1Ref={addressLine1Ref}
      managerName={managerName}
      managerEmail={managerEmail}
      managerPassword={managerPassword}
      restaurantName={restaurantName}
      restaurantPhone={restaurantPhone}
      addressLine1={addressLine1}
      addressLine2={addressLine2}
      city={city}
      postCode={postCode}
      step={step}
      canAdvance={form.isStepValid}
      advanceStep={advanceStep}
      backStep={backStep}
      onSubmit={onSubmit}
    />
  );
};

export default RegisterRestaurantFormController;
