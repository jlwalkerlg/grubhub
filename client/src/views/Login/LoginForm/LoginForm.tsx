import React, { FC } from "react";

import { useDispatch } from "react-redux";

import { useForm } from "react-hook-form";

import AuthApi from "~/api/AuthApi";

import {
  combineRules,
  RequiredRule,
  EmailRule,
  PasswordRule,
} from "~/lib/Form/Rule";
import { User } from "~/models/User";
import { createLoginAction } from "~/store/auth/authActionCreators";

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

  const onSubmit = form.handleSubmit(async (data) => {
    const response = await AuthApi.login(data);

    if (response.isSuccess) {
      const userDto = response.data.data;

      dispatch(
        createLoginAction(
          new User(userDto.id, userDto.name, userDto.email, userDto.role)
        )
      );

      return;
    }

    if (response.isValidationError) {
      form.errors = response.validationErrors;
    } else {
      // TODO: toast
      alert(response.error);
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
          data-valid={form.errors.email}
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
          data-valid={form.errors.password}
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
        <button type="submit" className="btn btn-primary font-semibold w-full">
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
