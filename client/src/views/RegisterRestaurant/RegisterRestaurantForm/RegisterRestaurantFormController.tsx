import React from "react";
import router from "next/router";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

import { RegisterRestaurantCommand } from "~/api/restaurants/restaurantsApi";

import useAddressSearch from "~/services/geolocation/useAddressSearch";

import RegisterRestaurantForm, {
  StepOne,
  StepThree,
  StepTwo,
} from "./RegisterRestaurantForm";
import useAuth from "~/store/auth/useAuth";
import { useForm } from "react-hook-form";
import useRestaurants from "~/store/restaurants/useRestaurants";

const MySwal = withReactContent(Swal);

const RegisterRestaurantFormController: React.FC = () => {
  const restaurants = useRestaurants();
  const [error, setError] = React.useState<string>(null);

  const auth = useAuth();

  const step1 = useForm<StepOne>({
    defaultValues: {
      managerName: "",
      managerEmail: "",
      managerPassword: "",
    },
    mode: "onBlur",
    reValidateMode: "onChange",
  });

  const step2 = useForm<StepTwo>({
    defaultValues: {
      restaurantName: "",
      restaurantPhoneNumber: "",
    },
    mode: "onBlur",
    reValidateMode: "onChange",
  });

  const step3 = useForm<StepThree>({
    defaultValues: {
      address: "",
    },
    mode: "onBlur",
    reValidateMode: "onChange",
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

  const onSubmit = async () => {
    if (step3.formState.isSubmitting) return;

    setError(null);

    const command: RegisterRestaurantCommand = {
      ...step1.getValues(),
      ...step2.getValues(),
      ...step3.getValues(),
    };

    const result = await restaurants.register(command);

    if (!result.isSuccess) {
      setError(result.error.message);

      if (result.error.isValidationError) {
        let invalidStep = 3;

        for (const field in result.error.errors) {
          if (
            Object.prototype.hasOwnProperty.call(result.error.errors, field)
          ) {
            const message = result.error.errors[field];

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

      return;
    }

    try {
      await Promise.all([
        MySwal.fire({
          title: <p>Thanks For Registering!</p>,
          text:
            "Your application to register your restaurant has been successfully recieved! We will review the application and get you up and running as soon as we can! Keep an eye on your emails for updates.",
          icon: "success",
          allowOutsideClick: false,
          allowEscapeKey: false,
          allowEnterKey: false,
          showConfirmButton: true,
        }),
        auth.login({
          email: command.managerEmail,
          password: command.managerPassword,
        }),
      ]);

      router.push("/dashboard");
    } catch (e) {
      router.push("/login");
    }
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
      error={error}
    />
  );
};

export default RegisterRestaurantFormController;
