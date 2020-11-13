import Router from "next/router";
import React from "react";
import { useForm } from "react-hook-form";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import useRegisterRestaurant, {
  RegisterRestaurantCommand,
} from "~/api/restaurants/useRegisterRestaurant";
import useAuth from "~/api/users/useAuth";
import useAddressSearch from "~/services/geolocation/useAddressSearch";
import RegisterRestaurantForm, {
  StepOne,
  StepThree,
  StepTwo,
} from "./RegisterRestaurantForm";

const MySwal = withReactContent(Swal);

const RegisterRestaurantFormController: React.FC = () => {
  const { isLoggedIn } = useAuth();

  if (isLoggedIn) {
    Router.push("/");
    return null;
  }

  const [
    register,
    { isError, error, isLoading, isSuccess },
  ] = useRegisterRestaurant();

  const step1 = useForm<StepOne>({
    defaultValues: {
      managerName: "",
      managerEmail: "",
      managerPassword: "",
    },
  });

  const step2 = useForm<StepTwo>({
    defaultValues: {
      restaurantName: "",
      restaurantPhoneNumber: "",
    },
  });

  const step3 = useForm<StepThree>({
    defaultValues: {
      address: "",
    },
  });

  const {
    results: addressSearchResults,
    address,
    onSelectAddress,
  } = useAddressSearch(step3.watch("address"));

  React.useEffect(() => {
    if (address !== null) {
      step3.setValue("address", address);
    }
  }, [address]);

  const [step, setStep] = React.useState(1);

  const advanceStep = () => {
    setStep(step + 1);
  };

  const backStep = () => {
    setStep(step - 1);
  };

  React.useEffect(() => {
    if (isLoading) return;

    if (isError) {
      if (error.isValidationError) {
        let invalidStep = 3;

        for (const field in error.errors) {
          if (Object.prototype.hasOwnProperty.call(error.errors, field)) {
            const message = error.errors[field];

            if (Object.keys(step1.getValues()).includes(field)) {
              invalidStep = 1;
              step1.setError(field as keyof StepOne, { message });
            } else if (Object.keys(step2.getValues()).includes(field)) {
              invalidStep = Math.min(invalidStep, 2);
              step2.setError(field as keyof StepTwo, { message });
            } else if (Object.keys(step3.getValues()).includes(field)) {
              step3.setError(field as keyof StepThree, { message });
            }
          }
        }

        if (invalidStep === 1) {
          setStep(1);
        } else if (invalidStep === 2) {
          setStep(2);
        }
      }
    } else if (isSuccess) {
      MySwal.fire({
        title: <p>Thanks For Registering!</p>,
        text:
          "Your application to register your restaurant has been successfully recieved! We will review the application and get you up and running as soon as we can! Keep an eye on your emails for updates.",
        icon: "success",
        allowOutsideClick: false,
        allowEscapeKey: false,
        allowEnterKey: false,
        showConfirmButton: true,
      }).then(() => {
        Router.push("/login");
      });
    }
  }, [isLoading]);

  const onSubmit = async () => {
    if (step3.formState.isSubmitting) return;

    const command: RegisterRestaurantCommand = {
      ...step1.getValues(),
      ...step2.getValues(),
      ...step3.getValues(),
    };

    await register(command);
  };

  return (
    <RegisterRestaurantForm
      addressSearchResults={addressSearchResults}
      onSelectAddress={onSelectAddress}
      step={step}
      step1={step1}
      step2={step2}
      step3={step3}
      advanceStep={advanceStep}
      backStep={backStep}
      onSubmit={onSubmit}
      isError={isError}
      error={error?.message}
    />
  );
};

export default RegisterRestaurantFormController;
