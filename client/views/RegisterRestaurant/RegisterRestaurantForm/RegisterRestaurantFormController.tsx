import React, {
  FC,
  FormEvent,
  useState,
  useMemo,
  SyntheticEvent,
  useEffect,
} from "react";
import RegisterRestaurantForm from "./RegisterRestaurantForm";
import { useFormComponent } from "~/lib/Form/useFormComponent";
import {
  RequiredRule,
  PasswordRule,
  EmailRule,
  PhoneRule,
  PostCodeRule,
} from "~/lib/Form/Rule";
import { CompositeForm, Form } from "~/lib/Form/Form";

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

  const [step, setStep] = useState(1);

  const form = useMemo(
    () =>
      new CompositeForm([
        new Form({ managerName, managerEmail, managerPassword }),
        new Form({ restaurantName, restaurantPhone }),
        new Form({ addressLine1, addressLine2, city, postCode }),
      ]),
    [
      managerName,
      managerEmail,
      managerPassword,
      restaurantName,
      restaurantPhone,
      addressLine1,
      addressLine2,
      city,
      postCode,
    ]
  );

  const [canAdvance, setCanAdvance] = useState(() => form.validateForm(0));

  useEffect(() => {
    setCanAdvance(form.validateForm(step - 1));
  }, [form]);

  function advanceStep(e: SyntheticEvent) {
    e.preventDefault();

    if (canAdvance) {
      setStep(step + 1);
    }
  }

  function backStep() {
    setStep(step - 1);
  }

  function onSubmit(e: FormEvent) {
    e.preventDefault();

    console.log("values", form.values);
  }

  return (
    <RegisterRestaurantForm
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
      canAdvance={canAdvance}
      advanceStep={advanceStep}
      backStep={backStep}
      onSubmit={onSubmit}
    />
  );
};

export default RegisterRestaurantFormController;
