import React, { FC, FormEvent, useState, MouseEvent, useMemo } from "react";
import RegisterRestaurantForm from "./RegisterRestaurantForm";
import { useFormComponent } from "~/lib/Form/useFormComponent";
import { RequiredRule, MinLengthRule } from "~/lib/Form/Rule";
import { CompositeForm, Form } from "~/lib/Form/Form";

const RegisterRestaurantFormController: FC = () => {
  const managerName = useFormComponent("", [
    new RequiredRule(),
    new MinLengthRule(5),
  ]);
  const managerEmail = useFormComponent("");
  const restaurantName = useFormComponent("");
  const restaurantPhone = useFormComponent("");
  const addressLine1 = useFormComponent("");
  const addressLine2 = useFormComponent("");
  const city = useFormComponent("");
  const postCode = useFormComponent("");

  const [step, setStep] = useState(1);

  const form = useMemo(
    () =>
      new CompositeForm([
        new Form({ managerName, managerEmail }),
        new Form({ restaurantName, restaurantPhone }),
        new Form({ addressLine1, addressLine2, city, postCode }),
      ]),
    [
      managerName,
      managerEmail,
      restaurantName,
      restaurantPhone,
      addressLine1,
      addressLine2,
      city,
      postCode,
    ]
  );

  function advanceStep(e: MouseEvent) {
    e.preventDefault();

    if (form.validateForm(step - 1)) {
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
      restaurantName={restaurantName}
      restaurantPhone={restaurantPhone}
      addressLine1={addressLine1}
      addressLine2={addressLine2}
      city={city}
      postCode={postCode}
      step={step}
      advanceStep={advanceStep}
      backStep={backStep}
      onSubmit={onSubmit}
    />
  );
};

export default RegisterRestaurantFormController;
