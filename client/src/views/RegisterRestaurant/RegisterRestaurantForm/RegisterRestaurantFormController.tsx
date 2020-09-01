import React, {
  FC,
  FormEvent,
  useState,
  SyntheticEvent,
  useEffect,
  KeyboardEvent,
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
  const managerName = useFormComponent("Jordan Walker", [new RequiredRule()]);
  const managerEmail = useFormComponent("walker.jlg@gmail.com", [
    new RequiredRule(),
    new EmailRule(),
  ]);
  const managerPassword = useFormComponent("password123", [
    new RequiredRule(),
    new PasswordRule(),
  ]);
  const restaurantName = useFormComponent("Chow Main", [new RequiredRule()]);
  const restaurantPhoneNumber = useFormComponent("01234567890", [
    new RequiredRule(),
    new PhoneRule(),
  ]);
  const addressLine1 = useFormComponent("19 Bodmin Avenue", [
    new RequiredRule(),
  ]);
  const addressLine2 = useFormComponent("");
  const town = useFormComponent("Shipley", [new RequiredRule()]);
  const postCode = useFormComponent("BD181LT", [
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
      town.setValue(address.town);
      postCode.setValue(address.postCode);
    }
  }, [address]);

  const [step, setStep] = useState(3);

  const form = useCompositeForm(
    [
      { managerName, managerEmail, managerPassword },
      { restaurantName, restaurantPhoneNumber },
      { addressLine1, addressLine2, town, postCode },
    ],
    step
  );

  const advanceStep = () => {
    if (form.validateStep()) {
      setStep(step + 1);
    }
  };

  const backStep = () => {
    setStep(step - 1);
  };

  const onAdvanceStep = (e: SyntheticEvent) => {
    e.preventDefault();

    advanceStep();
  };

  const onBackStep = (e: SyntheticEvent) => {
    e.preventDefault();

    backStep();
  };

  const [isSubmitting, setIsSubmitting] = React.useState(false);

  const submit = async () => {
    if (!form.isValid) return;

    setIsSubmitting(true);

    await restaurantsApi.register({
      managerName: managerName.value,
      managerEmail: managerEmail.value,
      managerPassword: managerPassword.value,
      restaurantName: restaurantName.value,
      restaurantPhoneNumber: restaurantPhoneNumber.value,
      addressLine1: addressLine1.value,
      addressLine2: addressLine2.value,
      town: town.value,
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
  };

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();

    submit();
  };

  const onFormKeydown = (e: KeyboardEvent) => {
    if (e.key === "Enter") {
      if (step === 3) {
        submit();
      } else {
        advanceStep();
      }
    }
  };

  return (
    <RegisterRestaurantForm
      isSubmitting={isSubmitting}
      addressSearchResults={addressSearchResults}
      onSelectAddress={onSelectAddress}
      managerName={managerName}
      managerEmail={managerEmail}
      managerPassword={managerPassword}
      restaurantName={restaurantName}
      restaurantPhoneNumber={restaurantPhoneNumber}
      addressLine1={addressLine1}
      addressLine2={addressLine2}
      town={town}
      postCode={postCode}
      step={step}
      canAdvance={!isSubmitting && form.isStepValid}
      onAdvanceStep={onAdvanceStep}
      onBackStep={onBackStep}
      onSubmit={onSubmit}
      onFormKeydown={onFormKeydown}
    />
  );
};

export default RegisterRestaurantFormController;
