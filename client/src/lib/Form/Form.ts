import { FormComponent } from "./useFormComponent";

export interface IForm {
  validate(): boolean;
  values: { [field: string]: string };
}

export class Form implements IForm {
  constructor(readonly components: { [field: string]: FormComponent }) {}

  validate(): boolean {
    for (const component of Object.values(this.components)) {
      if (!component.validate()) return false;
    }

    return true;
  }

  get values() {
    const values = {};

    for (const field in this.components) {
      if (Object.prototype.hasOwnProperty.call(this.components, field)) {
        const component = this.components[field];
        values[field] = component.value;
      }
    }

    return values;
  }
}

export class CompositeForm implements IForm {
  constructor(readonly forms: Form[]) {}

  validate(): boolean {
    for (const form of this.forms) {
      if (!form.validate()) return false;
    }

    return true;
  }

  validateForm(x: number): boolean {
    return this.forms[x].validate();
  }

  get values() {
    let values = {};

    for (const form of this.forms) {
      values = { ...values, ...form.values };
    }

    return values;
  }
}
