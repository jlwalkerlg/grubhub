import React, {
  FC,
  FormEvent,
  useState,
  SyntheticEvent,
  useEffect,
} from "react";

import {
  RequiredRule,
  PasswordRule,
  EmailRule,
  PhoneRule,
  PostCodeRule,
} from "~/lib/Form/Rule";
import { useFormComponent } from "~/lib/Form/useFormComponent";
import useAddressSearch from "~/lib/AddressSearch/useAddressSearch";

import RegisterRestaurantForm from "./RegisterRestaurantForm";
import useForm from "~/lib/Form/useForm";
import useCompositeForm from "~/lib/Form/useCompositeForm";

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
  } = useAddressSearch(addressLine1.value);

  useEffect(() => {
    if (address !== null) {
      addressLine1.setValue(address.addressLine1);
      addressLine2.setValue(address.addressLine2);
      city.setValue(address.city);
      postCode.setValue(address.postCode);
    }
  }, [address]);

  const [step, setStep] = useState(3);

  const step1 = useForm({ managerName, managerEmail, managerPassword });
  const step2 = useForm({ restaurantName, restaurantPhone });
  const step3 = useForm({ addressLine1, addressLine2, city, postCode });
  const form = useCompositeForm([step1, step2, step3]);

  function advanceStep(e: SyntheticEvent) {
    e.preventDefault();

    if (form.isValid) {
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

  return (
    <RegisterRestaurantForm
      addressSearchResults={addressSearchResults}
      onSelectAddress={onSelectAddress}
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
      canAdvance={form.isValid}
      advanceStep={advanceStep}
      backStep={backStep}
      onSubmit={onSubmit}
    />
  );
};

export default RegisterRestaurantFormController;
