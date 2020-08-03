import React, { FC } from "react";

import { FormComponent } from "~/lib/Form/useFormComponent";

interface Props {
  component: FormComponent;
  className: string | null;
}

const FormError: FC<Props> = ({ component, className }) => {
  if (!component.touched || component.valid === null) return null;

  return <p className={`form-error ${className || ""}`}>{component.error}</p>;
};

export default FormError;
