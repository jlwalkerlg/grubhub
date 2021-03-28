import Head from "next/head";
import React, { FC } from "react";
import useAuth from "~/api/users/useAuth";
import useChangePassword from "~/api/users/useChangePassword";
import useUpdateAccountDetails from "~/api/users/useUpdateAccountDetails";
import useUpdateDeliveryAddress from "~/api/users/useUpdateDeliveryAddress";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import { AuthLayout } from "~/components/Layout/Layout";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";

const CustomerDetailsForm: FC = () => {
  const { user } = useAuth();

  const form = useForm({
    defaultValues: {
      firstName: user.firstName,
      lastName: user.lastName,
      mobileNumber: user.mobileNumber,
    },
  });

  const rules = useRules({
    firstName: (b) => b.required(),
    lastName: (b) => b.required(),
    mobileNumber: (b) => b.required().mobile(),
  });

  const { mutateAsync: update } = useUpdateAccountDetails();

  const onSubmit = form.handleSubmit(async (data) => {
    await update(data);
  });

  return (
    <form className="bg-white rounded shadow-sm p-4 mt-2" onSubmit={onSubmit}>
      {form.error && !form.hasValidationErrors && (
        <div className="my-6">
          <ErrorAlert message={form.error.message} />
        </div>
      )}

      {form.isSuccess && (
        <div className="my-6">
          <SuccessAlert message="Account details updated." />
        </div>
      )}

      <div>
        <label className="label" htmlFor="firstName">
          First name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.firstName,
          })}
          className="input bg-white"
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
          Last name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.lastName,
          })}
          className="input bg-white"
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
        <label className="label" htmlFor="mobileNumber">
          Mobile number <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.mobileNumber,
          })}
          className="input bg-white"
          type="text"
          name="mobileNumber"
          id="mobileNumber"
          data-invalid={!!form.errors.mobileNumber}
        />
        {form.errors.mobileNumber && (
          <p className="form-error mt-1">{form.errors.mobileNumber.message}</p>
        )}
      </div>

      <button className="btn btn-primary w-full mt-6" disabled={form.isLoading}>
        Save details
      </button>
    </form>
  );
};

const DeliveryAddressForm: FC = () => {
  const { user } = useAuth();

  const form = useForm({
    defaultValues: {
      addressLine1: user.addressLine1,
      addressLine2: user.addressLine2,
      city: user.city,
      postcode: user.postcode,
    },
  });

  const rules = useRules({
    addressLine1: (b) => b.required(),
    city: (b) => b.required(),
    postcode: (b) => b.required().postcode(),
  });

  const { mutateAsync: update } = useUpdateDeliveryAddress();

  const onSubmit = form.handleSubmit(async (data) => {
    await update(data);
  });

  return (
    <form className="bg-white rounded shadow-sm p-4 mt-2" onSubmit={onSubmit}>
      {form.error && !form.hasValidationErrors && (
        <div className="my-6">
          <ErrorAlert message={form.error.message} />
        </div>
      )}

      {form.isSuccess && (
        <div className="my-6">
          <SuccessAlert message="Delivery address updated." />
        </div>
      )}

      <div>
        <label className="label" htmlFor="addressLine1">
          Address <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.addressLine1,
          })}
          className="input bg-white"
          type="text"
          name="addressLine1"
          id="addressLine1"
          data-invalid={!!form.errors.addressLine1}
        />
        {form.errors.addressLine1 && (
          <p className="form-error mt-1">{form.errors.addressLine1.message}</p>
        )}
      </div>

      <div className="mt-4">
        <input
          ref={form.register}
          className="input bg-white"
          type="text"
          name="addressLine2"
          id="addressLine2"
          data-invalid={!!form.errors.addressLine2}
        />
        {form.errors.addressLine2 && (
          <p className="form-error mt-1">{form.errors.addressLine2.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="city">
          City <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.city,
          })}
          className="input bg-white"
          type="text"
          name="city"
          id="city"
          data-invalid={!!form.errors.city}
        />
        {form.errors.city && (
          <p className="form-error mt-1">{form.errors.city.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="postcode">
          Postcode <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.postcode,
          })}
          className="input bg-white"
          type="text"
          name="postcode"
          id="postcode"
          data-invalid={!!form.errors.postcode}
        />
        {form.errors.postcode && (
          <p className="form-error mt-1">{form.errors.postcode.message}</p>
        )}
      </div>

      <button className="btn btn-primary w-full mt-6" disabled={form.isLoading}>
        Save address
      </button>
    </form>
  );
};

const ChangePasswordForm: FC = () => {
  const form = useForm({
    defaultValues: {
      currentPassword: "",
      newPassword: "",
      confirmNewPassword: "",
    },
  });

  const rules = useRules({
    currentPassword: (b) => b.required(),
    newPassword: (b) => b.required().password(),
    confirmNewPassword: (b) =>
      b
        .required()
        .match(() => form.getValues().newPassword, "Passwords must match."),
  });

  const { mutateAsync: update } = useChangePassword();

  const onSubmit = form.handleSubmit(
    async ({ confirmNewPassword, ...command }) => {
      await update(command);
      form.reset();
    }
  );

  return (
    <form className="bg-white rounded shadow-sm p-4 mt-2" onSubmit={onSubmit}>
      {form.error && !form.hasValidationErrors && (
        <div className="my-6">
          <ErrorAlert message={form.error.message} />
        </div>
      )}

      {form.isSuccess && (
        <div className="my-6">
          <SuccessAlert message="Password updated." />
        </div>
      )}

      <div>
        <label className="label" htmlFor="currentPassword">
          Current Password <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.currentPassword,
          })}
          className="input bg-white"
          type="password"
          name="currentPassword"
          id="currentPassword"
          data-invalid={!!form.errors.currentPassword}
        />
        {form.errors.currentPassword && (
          <p className="form-error mt-1">
            {form.errors.currentPassword.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="newPassword">
          New Password <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.newPassword,
          })}
          className="input bg-white"
          type="password"
          name="newPassword"
          id="newPassword"
          data-invalid={!!form.errors.newPassword}
        />
        {form.errors.newPassword && (
          <p className="form-error mt-1">{form.errors.newPassword.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="confirmNewPassword">
          Confirm New Password <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.confirmNewPassword,
          })}
          className="input bg-white"
          type="password"
          name="confirmNewPassword"
          id="confirmNewPassword"
          data-invalid={!!form.errors.confirmNewPassword}
        />
        {form.errors.confirmNewPassword && (
          <p className="form-error mt-1">
            {form.errors.confirmNewPassword.message}
          </p>
        )}
      </div>

      <button className="btn btn-primary w-full mt-6" disabled={form.isLoading}>
        Change password
      </button>
    </form>
  );
};

const AccountPage: FC = () => {
  return (
    <div className="mt-4 bg-gray-50 rounded py-8 px-24 max-w-2xl mx-auto">
      <h1 className="font-semibold text-4xl text-gray-700">Account details</h1>

      <div className="mt-4">
        <h2 className="text-lg font-semibold text-gray-700">Your details</h2>
        <CustomerDetailsForm />
      </div>

      <div className="mt-6">
        <h2 className="text-lg font-semibold text-gray-700">
          Delivery address
        </h2>
        <DeliveryAddressForm />
      </div>

      <div className="mt-6">
        <h2 className="text-lg font-semibold text-gray-700">Change password</h2>
        <ChangePasswordForm />
      </div>
    </div>
  );
};

const AccountLayout: FC = () => {
  return (
    <AuthLayout title="Your Account">
      <Head>
        <style
          dangerouslySetInnerHTML={{
            __html: `
            body {
              background-color: #fff !important;
            }
          `,
          }}
        ></style>
      </Head>
      <AccountPage />
    </AuthLayout>
  );
};

export default AccountLayout;
