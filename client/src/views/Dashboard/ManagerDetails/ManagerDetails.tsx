import { NextPage } from "next";
import React from "react";
import { useForm } from "react-hook-form";
import useAuth from "~/api/users/useAuth";
import useUpdateUserDetails from "~/api/users/useUpdateUserDetails";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import { useRules } from "~/services/useRules";
import { setFormErrors } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";

const UpdateManagerDetailsForm = () => {
  const { user } = useAuth();

  const form = useForm({
    defaultValues: {
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
    },
  });

  const rules = useRules({
    firstName: (builder) => builder.required(),
    lastName: (builder) => builder.required(),
    email: (builder) => builder.required().email(),
  });

  const {
    mutate: updateUserDetails,
    isLoading,
    error,
    isSuccess,
  } = useUpdateUserDetails();

  const onSubmit = form.handleSubmit(async (data) => {
    if (isLoading) return;

    updateUserDetails(data, {
      onError: (error) => {
        if (error.isValidationError) {
          setFormErrors(error.errors, form);
        }
      },
    });
  });

  return (
    <form onSubmit={onSubmit}>
      {error && (
        <div className="my-6">
          <ErrorAlert message={error.detail} />
        </div>
      )}

      {isSuccess && (
        <div className="my-6">
          <SuccessAlert message="Manager details updated!" />
        </div>
      )}

      <div className="mt-4">
        <label className="label" htmlFor="firstName">
          First Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.firstName,
          })}
          className="input"
          type="text"
          name="firstName"
          id="firstName"
          data-invalid={!!form.errors.firstName}
        />
        {form.errors.firstName && (
          <p className="form-error mt-1">{form.errors.firstName.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="lastName">
          Last Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.lastName,
          })}
          className="input"
          type="text"
          name="lastName"
          id="lastName"
          data-invalid={!!form.errors.lastName}
        />
        {form.errors.lastName && (
          <p className="form-error mt-1">{form.errors.lastName.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="email">
          Email <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.email,
          })}
          className="input"
          type="text"
          name="email"
          id="email"
          data-invalid={!!form.errors.email}
        />
        {form.errors.email && (
          <p className="form-error mt-1">{form.errors.email.message}</p>
        )}
      </div>

      <div className="mt-4">
        <button
          type="submit"
          disabled={isLoading}
          className="btn btn-primary font-semibold w-full"
        >
          Update
        </button>
      </div>
    </form>
  );
};

const ManagerDetails: NextPage = () => {
  const { isLoggedIn } = useAuth();

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Manager Details
      </h2>

      {isLoggedIn && <UpdateManagerDetailsForm />}
    </DashboardLayout>
  );
};

export default ManagerDetails;
