import React, { FC } from "react";

import router from "next/router";

import { useDispatch } from "react-redux";

import { useForm } from "react-hook-form";

import AuthApi from "~/api/authApi";

import {
  combineRules,
  RequiredRule,
  EmailRule,
  PasswordRule,
} from "~/lib/Form/Rule";
import { User, UserRole } from "~/models/User";
import { createLoginAction } from "~/store/auth/authActionCreators";
import { ErrorAlert } from "~/components/Alert/Alert";

interface FormValues {
  email: string;
  password: string;
}

const LoginForm: FC = () => {
  const dispatch = useDispatch();

  const form = useForm<FormValues>({
    defaultValues: { email: "", password: "" },
    mode: "onBlur",
    reValidateMode: "onChange",
  });

  const [error, setError] = React.useState<string>(null);

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    setError(null);

    const response = await AuthApi.login(data);

    if (response.isSuccess) {
      const userDto = response.data.data;
      const user = new User(
        userDto.id,
        userDto.name,
        userDto.email,
        userDto.role
      );

      dispatch(createLoginAction(user));

      if (user.role === UserRole.RestaurantManager) {
        router.push("/dashboard");
      } else {
        router.push("/");
      }

      return;
    }

    if (response.isValidationError) {
      form.errors = response.validationErrors;
    } else {
      setError(response.error);
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
