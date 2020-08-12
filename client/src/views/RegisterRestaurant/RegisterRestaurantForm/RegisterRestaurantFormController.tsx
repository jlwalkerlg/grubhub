import React, {
  FC,
  FormEvent,
  useState,
  SyntheticEvent,
  useEffect,
} from "react";
import router from "next/router";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

import restaurantsApi from "~/api/RestaurantsApi";

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

import RegisterRestaurantForm from "./RegisterRestaurantForm";

const MySwal = withReactContent(Swal);

const RegisterRestaurantFormController: FC = () => {
  const managerName = useFormComponent("", [new RequiredRule()]);
  const managerEmail = useFormComponent("", [
    new RequiredRule(),
    new EmailRule(),
  ]);
  const managerPassword = useFormComponent("", [
    new RequiredRule(),
    new PasswordRule(),
  ]);
  const restaurantName = useFormComponent("", [new RequiredRule()]);
  const restaurantPhone = useFormComponent("", [
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

  const [isSubmitting, setIsSubmitting] = React.useState(false);
  async function onSubmit(e: FormEvent) {
    e.preventDefault();

    if (!form.isValid) return;

    setIsSubmitting(true);

    await restaurantsApi.register({
      managerName: managerName.value,
      managerEmail: managerEmail.value,
      managerPassword: managerPassword.value,
      restaurantName: restaurantName.value,
      restaurantPhone: restaurantPhone.value,
      addressLine1: addressLine1.value,
      addressLine2: addressLine2.value,
      city: city.value,
      postCode: postCode.value,
    });

    setIsSubmitting(false);

    await MySwal.fire({
      title: <p>Thanks For Registering!</p>,
      text:
        "Your application to register your restaurant has been successfully recieved! We will review the application and get you up and running as soon as we can! Keep an eye on your emails for updates.",
      icon: "success",
      allowOutsideClick: false,
      allowEscapeKey: false,
      allowEnterKey: false,
      showConfirmButton: true,
    });

    router.push("/");
  }

  return (
    <RegisterRestaurantForm
      isSubmitting={isSubmitting}
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
      canAdvance={!isSubmitting && form.isStepValid}
      advanceStep={advanceStep}
      backStep={backStep}
      onSubmit={onSubmit}
    />
  );
};

export default RegisterRestaurantFormController;
