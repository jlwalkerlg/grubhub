import { NextPage } from "next";
import React from "react";
import { useForm } from "react-hook-form";
import { UpdateUserDetailsCommand } from "~/api/users/userApi";
import { ErrorAlert } from "~/components/Alert/Alert";
import { combineRules, EmailRule, RequiredRule } from "~/lib/Form/Rule";
import useAuth from "~/store/auth/useAuth";
import { setFormErrors } from "~/utils/setFormErrors";
import { DashboardLayout } from "./DashboardLayout";

const ManagerDetails: NextPage = () => {
  const auth = useAuth();
  const [error, setError] = React.useState<string>(null);

  const form = useForm<UpdateUserDetailsCommand>({
    defaultValues: {
      name: auth.user.name,
      email: auth.user.email,
    },
    mode: "onBlur",
    reValidateMode: "onChange",
  });

  React.useEffect(() => {
    form.register("name", {
      validate: combineRules([new RequiredRule()]),
    });
    form.register("email", {
      validate: combineRules([new RequiredRule(), new EmailRule()]),
    });
  }, [form.register]);

  const onSubmit = async (command: UpdateUserDetailsCommand) => {
    if (form.formState.isSubmitting) return;

    setError(null);

    const result = await auth.updateUserDetails(command);

    if (!result.isSuccess) {
      setError(result.error.message);

      if (result.error.isValidationError) {
        setFormErrors(result.error.errors, form);
      }

      return;
    }
  };

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Manager Details
      </h2>

      <form onSubmit={form.handleSubmit(onSubmit)}>
        {error && (
          <div className="my-6">
            <ErrorAlert message={error} />
          </div>
        )}

        <div className="mt-4">
          <label className="label" htmlFor="name">
            Name <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register}
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
            ref={form.register}
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
            Submit
          </button>
        </div>
      </form>
    </DashboardLayout>
  );
};

export default ManagerDetails;
