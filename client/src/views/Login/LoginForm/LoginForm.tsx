import React, { FC } from "react";
import router from "next/router";
import { useForm } from "react-hook-form";

import {
  combineRules,
  RequiredRule,
  EmailRule,
  PasswordRule,
} from "~/lib/Form/Rule";
import { ErrorAlert } from "~/components/Alert/Alert";
import useAuth from "~/store/auth/useAuth";
import { LoginCommand } from "~/api/users/userApi";

const LoginForm: FC = () => {
  const auth = useAuth();

  const form = useForm<LoginCommand>({
    defaultValues: { email: "", password: "" },
    mode: "onBlur",
    reValidateMode: "onChange",
  });

  const [error, setError] = React.useState<string>(null);

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    setError(null);

    const result = await auth.login(data);

    if (result.isSuccess) {
      const user = result.data;

      if (user.role === "RestaurantManager") {
        router.push("/dashboard");
      } else {
        router.push("/");
      }

      return;
    }

    setError(result.error.message);

    if (result.error.isValidationError) {
      for (const field in result.error.errors) {
        if (Object.prototype.hasOwnProperty.call(result.error.errors, field)) {
          const message = result.error.errors[field];
          form.setError(field as keyof LoginCommand, { message });
        }
      }
    }
  });

  React.useEffect(() => {
    form.register("email", {
      validate: combineRules([new RequiredRule(), new EmailRule()]),
    });
    form.register("password", {
      validate: combineRules([new RequiredRule(), new PasswordRule()]),
    });
  }, [form.register]);

  return (
    <form onSubmit={onSubmit}>
      {error && (
        <div className="my-6">
          <ErrorAlert message={error} />
        </div>
      )}

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
        <label className="label" htmlFor="password">
          Password <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register}
          className="input"
          type="password"
          name="password"
          id="password"
          data-invalid={!!form.errors.password}
        />
        {form.errors.password && (
          <p className="form-error mt-1">{form.errors.password.message}</p>
        )}
      </div>

      <div className="mt-2">
        <div className="text-right">
          <a href="/forgot-password" className="text-sm text-blue-700">
            Forgot Password?
          </a>
        </div>
      </div>

      <div className="mt-4">
        <button
          type="submit"
          disabled={form.formState.isSubmitting}
          className="btn btn-primary font-semibold w-full"
        >
          Login
        </button>
      </div>

      <div className="mt-4 text-center">
        <p className="text-sm text-gray-600">
          Don't have an account?{" "}
          <a href="/register" className="underline text-blue-700">
            Sign up
          </a>
        </p>
      </div>
    </form>
  );
};

export default LoginForm;
