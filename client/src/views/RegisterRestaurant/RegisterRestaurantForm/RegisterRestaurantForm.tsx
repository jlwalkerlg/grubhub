import Router from "next/router";
import React, { FC, useState } from "react";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import useRegisterRestaurant, {
  RegisterRestaurantCommand,
} from "~/api/restaurants/useRegisterRestaurant";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import RegisterRestaurantFormStepOne from "./RegisterRestaurantFormStepOne";
import RegisterRestaurantFormStepThree from "./RegisterRestaurantFormStepThree";
import RegisterRestaurantFormStepTwo from "./RegisterRestaurantFormStepTwo";

const MySwal = withReactContent(Swal);

const RegisterRestaurantForm: FC = () => {
  const { isLoggedIn } = useAuth();

  if (isLoggedIn) {
    Router.push("/");
  }

  const [
    register,
    { isError, error, isLoading, isSuccess },
  ] = useRegisterRestaurant();

  const [values, setValues] = useState<RegisterRestaurantCommand>({
    managerFirstName: "",
    managerLastName: "",
    managerEmail: "",
    managerPassword: "",
    restaurantName: "",
    restaurantPhoneNumber: "",
    addressLine1: "",
    addressLine2: "",
    city: "",
    postcode: "",
  });

  const [errors, setErrors] = useState<
    { [K in keyof RegisterRestaurantCommand]?: string[] }
  >({});

  const [step, setStep] = useState(1);

  const advanceStep = (data: any) => {
    setValues({ ...values, ...data });
    setStep(step + 1);
  };

  const backStep = (data: any) => {
    setValues({ ...values, ...data });
    setStep(step - 1);
  };

  const onSubmit = async (data: any) => {
    if (isLoading || isSuccess) return;

    const command = { ...values, ...data };

    setValues(command);

    await register(command, {
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
          setErrors(error.errors);

          for (const field of [
            "managerFirstName",
            "managerLastName",
            "managerEmail",
            "managerPassword",
          ]) {
            if (error.errors.hasOwnProperty(field)) {
              setStep(1);
              return;
            }
          }

          for (const field of ["restaurantName", "restaurantPhoneNumber"]) {
            if (error.errors.hasOwnProperty(field)) {
              setStep(2);
              return;
            }
          }
        }
      },
    });
  };

  return (
    <div>
      {isError && (
        <div className="my-6">
          <ErrorAlert message={error?.message} />
        </div>
      )}

      {step === 1 && (
        <RegisterRestaurantFormStepOne
          defaults={values}
          errors={errors}
          advanceStep={advanceStep}
        />
      )}
      {step === 2 && (
        <RegisterRestaurantFormStepTwo
          defaults={values}
          errors={errors}
          backStep={backStep}
          advanceStep={advanceStep}
        />
      )}
      {step === 3 && (
        <RegisterRestaurantFormStepThree
          isSubmitting={isLoading}
          defaults={values}
          errors={errors}
          backStep={backStep}
          onSubmit={onSubmit}
        />
      )}
    </div>
  );
};

export default RegisterRestaurantForm;
