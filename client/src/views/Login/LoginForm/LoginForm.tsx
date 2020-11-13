import Link from "next/link";
import React from "react";
import { useForm } from "react-hook-form";
import useLogin, { LoginCommand } from "~/api/users/useLogin";
import { ErrorAlert } from "~/components/Alert/Alert";
import {
  combineRules,
  EmailRule,
  PasswordRule,
  RequiredRule,
} from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";

const LoginForm: React.FC = () => {
  const form = useForm<LoginCommand>({
    defaultValues: { email: "", password: "" },
  });

  const [login, { isError, error }] = useLogin();

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    await login(data, {
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
          <ErrorAlert message={error.message} />
        </div>
      )}

      <div className="mt-4">
        <label className="label" htmlFor="email">
          Email <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: combineRules([new RequiredRule(), new EmailRule()]),
          })}
          className="input"
          type="email"
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
          ref={form.register({
            validate: combineRules([new RequiredRule(), new PasswordRule()]),
          })}
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
          <Link href="/forgot-password">
            <a className="text-sm text-blue-700">Forgot Password?</a>
          </Link>
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
          <Link href="/register">
            <a className="underline text-blue-700">Sign up</a>
          </Link>
        </p>
      </div>
    </form>
  );
};

export default LoginForm;
