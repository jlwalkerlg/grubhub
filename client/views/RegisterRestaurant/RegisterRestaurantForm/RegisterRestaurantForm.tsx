import React, { FC, FormEvent } from "react";
import { FormComponent } from "~/lib/Form/useFormComponent";

export interface Props {
  managerName: FormComponent;
  managerEmail: FormComponent;
  restaurantName: FormComponent;
  restaurantPhone: FormComponent;
  addressLine1: FormComponent;
  addressLine2: FormComponent;
  city: FormComponent;
  postCode: FormComponent;
  onSubmit(e: FormEvent): void;
}

const RegisterRestaurantForm: FC<Props> = ({
  managerName,
  managerEmail,
  restaurantName,
  restaurantPhone,
  addressLine1,
  addressLine2,
  city,
  postCode,
  onSubmit,
}) => {
  return (
    <form action="/restaurants/register" method="POST" onSubmit={onSubmit}>
      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Manager Details
      </p>

      <div className="mt-6">
        <label className="label" htmlFor="managerName">
          Manager Name <span className="text-primary">*</span>
        </label>
        <input
          {...managerName.props}
          className="input"
          type="text"
          name="managerName"
          id="managerName"
        />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="managerEmail">
          Manager Email <span className="text-primary">*</span>
        </label>
        <input
          {...managerEmail.props}
          className="input"
          type="email"
          name="managerEmail"
          id="managerEmail"
        />
      </div>

      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Details
      </p>

      <div className="mt-6">
        <label className="label" htmlFor="restaurantName">
          Name <span className="text-primary">*</span>
        </label>
        <input
          {...restaurantName.props}
          className="input"
          type="text"
          name="restaurantName"
          id="restaurantName"
        />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="restaurantPhone">
          Phone Number <span className="text-primary">*</span>
        </label>
        <input
          {...restaurantPhone.props}
          className="input"
          type="tel"
          name="restaurantPhone"
          id="restaurantPhone"
        />
      </div>

      <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
        Restaurant Address
      </p>

      <div className="mt-4">
        <label className="label" htmlFor="addressLine1">
          Address Line 1 <span className="text-primary">*</span>
        </label>
        <input
          {...addressLine1.props}
          className="input"
          type="text"
          name="addressLine1"
          id="addressLine1"
        />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="addressLine2">
          Address Line 2
        </label>
        <input
          {...addressLine2.props}
          className="input"
          type="text"
          name="addressLine2"
          id="addressLine2"
        />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="city">
          Town / City <span className="text-primary">*</span>
        </label>
        <input
          {...city.props}
          className="input"
          type="text"
          name="city"
          id="city"
        />
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="postCode">
          Post Code <span className="text-primary">*</span>
        </label>
        <input
          {...postCode.props}
          className="input"
          type="text"
          name="postCode"
          id="postCode"
        />
      </div>

      <div className="mt-8">
        <button
          className="btn btn-primary font-semibold w-full"
          onClick={onSubmit}
        >
          Register
        </button>
      </div>
    </form>
  );
};

export default RegisterRestaurantForm;