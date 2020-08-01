import React, { FC, FormEvent, useState, MouseEvent } from "react";
import RegisterRestaurantForm from "./RegisterRestaurantForm";
import { useFormComponent, FormComponent } from "~/lib/Form/useFormComponent";
import { RequiredRule, MinLengthRule } from "~/lib/Form/Rule";

class Form {
  constructor(readonly components: FormComponent[]) {}

  validate(): boolean {
    for (const component of this.components) {
      if (!component.validate()) return false;
    }

    return true;
  }
}

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

  const forms = [
    new Form([managerName, managerEmail]),
    new Form([restaurantName, restaurantPhone]),
    new Form([addressLine1, addressLine2, city, postCode]),
  ];

  function advanceStep(e: MouseEvent) {
    e.preventDefault();

    if (forms[step - 1].validate()) {
      setStep(step + 1);
    }
  }

  function backStep() {
    setStep(step - 1);
  }

  function onSubmit(e: FormEvent) {
    e.preventDefault();

    console.log("values", {
      managerName: managerName.value,
      managerEmail: managerEmail.value,
      restaurantName: restaurantName.value,
      restaurantPhone: restaurantPhone.value,
      addressLine1: addressLine1.value,
      addressLine2: addressLine2.value,
      city: city.value,
      postCode: postCode.value,
    });
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
