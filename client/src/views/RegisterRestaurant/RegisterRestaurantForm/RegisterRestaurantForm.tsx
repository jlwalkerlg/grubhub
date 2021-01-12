import Router from "next/router";
import React, { FC, useState } from "react";
import { useForm } from "react-hook-form";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import useRegisterRestaurant from "~/api/restaurants/useRegisterRestaurant";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import { setFormErrors } from "~/services/forms/setFormErrors";
import RegisterRestaurantFormStepOne from "./RegisterRestaurantFormStepOne";
import RegisterRestaurantFormStepThree from "./RegisterRestaurantFormStepThree";
import RegisterRestaurantFormStepTwo from "./RegisterRestaurantFormStepTwo";

const MySwal = withReactContent(Swal);

const RegisterRestaurantForm: FC = () => {
  const { isLoggedIn } = useAuth();

  if (isLoggedIn) {
    Router.push("/");
    return null;
  }

  const [register, { isError, error }] = useRegisterRestaurant();

  const step1 = useForm({
    defaultValues: {
      managerName: "",
      managerEmail: "",
      managerPassword: "",
    },
  });

  const step2 = useForm({
    defaultValues: {
      restaurantName: "",
      restaurantPhoneNumber: "",
    },
  });

  const step3 = useForm({
    defaultValues: {
      address: "",
    },
  });

  const [step, setStep] = useState(1);

  const advanceStep = () => {
    setStep(step + 1);
  };

  const backStep = () => {
    setStep(step - 1);
  };

  const onSubmit = async () => {
    if (step3.formState.isSubmitting) return;

    await register(
      {
        ...step1.getValues(),
        ...step2.getValues(),
        ...step3.getValues(),
      },
      {
        onSuccess: async () => {
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

          Router.push("/login");
        },

        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, step1);
            setFormErrors(error.errors, step2);
            setFormErrors(error.errors, step3);

            if (Object.keys(step1.errors).length > 0) {
              setStep(1);
            } else if (Object.keys(step2.errors).length > 0) {
              setStep(2);
            }
          }
        },
      }
    );
  };

  return (
    <div>
      {isError && (
        <div className="my-6">
          <ErrorAlert message={error?.message} />
        </div>
      )}

      <div className={step !== 1 ? "sr-only" : undefined}>
        <RegisterRestaurantFormStepOne form={step1} advanceStep={advanceStep} />
      </div>
      <div className={step !== 2 ? "sr-only" : undefined}>
        <RegisterRestaurantFormStepTwo
          form={step2}
          backStep={backStep}
          advanceStep={advanceStep}
        />
      </div>
      <div className={step !== 3 ? "sr-only" : undefined}>
        <RegisterRestaurantFormStepThree
          form={step3}
          backStep={backStep}
          onSubmit={onSubmit}
        />
      </div>
    </div>
  );
};

export default RegisterRestaurantForm;
