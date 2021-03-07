import { NextPage } from "next";
import React from "react";
import { useForm } from "react-hook-form";
import useAuth from "~/api/users/useAuth";
import useUpdateUserDetails from "~/api/users/useUpdateUserDetails";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import { combineRules, EmailRule, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
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

  const [
    updateUserDetails,
    { isError, error, isSuccess },
  ] = useUpdateUserDetails();

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    await updateUserDetails(data, {
      onError: (error) => {
        if (error.isValidationError) {
          setFormErrors(error.errors, form);
        }
      },
    });
  });

  return (
    <form onSubmit={onSubmit}>
      {isError && (
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
        <label className="label" htmlFor="name">
          Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: combineRules([new RequiredRule()]),
          })}
          className="input"
          type="text"
          name="name"
          id="name"
          data-invalid={!!form.errors.name}
        />
        {form.errors.name && (
          <p className="form-error mt-1">{form.errors.name.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="email">
          Email <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: combineRules([new RequiredRule(), new EmailRule()]),
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
          disabled={form.formState.isSubmitting}
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
