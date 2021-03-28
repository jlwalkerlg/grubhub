import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC } from "react";
import useAuth from "~/api/users/useAuth";
import useLogin from "~/api/users/useLogin";
import { ErrorAlert } from "~/components/Alert/Alert";
import Layout from "~/components/Layout/Layout";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";

const Login: FC = () => {
  const form = useForm({
    defaultValues: { email: "", password: "" },
  });

  const rules = useRules({
    email: (b) => b.required().email(),
    password: (b) => b.required(),
  });

  const { mutateAsync: login } = useLogin();

  const onSubmit = form.handleSubmit(async (data) => {
    await login(data);
  });

  const loginAsDemoCustomer = async () => {
    form.setValue("email", "joe.bloggs@gmail.com");
    form.setValue("password", "password123");
    await onSubmit();
  };

  const loginAsDemoManager = async () => {
    form.setValue("email", "mr.manager@gmail.com");
    form.setValue("password", "password123");
    await onSubmit();
  };

  return (
    <div className="mt-4 ring-1 ring-black ring-opacity-5 p-8 bg-white rounded max-w-5xl mx-auto">
      <h1 className="font-semibold text-2xl text-gray-800 text-center mt-2">
        Welcome back
      </h1>

      <form onSubmit={onSubmit}>
        {form.error && (
          <div className="my-6">
            <ErrorAlert message={form.error.message} />
          </div>
        )}

        <div className="mt-4">
          <label className="label" htmlFor="email">
            Email <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: rules.email,
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
              validate: rules.password,
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
            disabled={form.isLoading}
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

        <p className="text-center text-gray-500 text-sm mt-4">OR</p>

        <div className="mt-4 md:flex items-center">
          <button
            type="button"
            disabled={form.isLoading}
            className="block w-full btn bg-green-600 hover:bg-green-800 text-white font-semibold flex-1 md:mr-2"
            onClick={loginAsDemoCustomer}
          >
            Login as Demo Customer
          </button>

          <button
            type="button"
            disabled={form.isLoading}
            className="block w-full mt-2 md:mt-0 btn border border-green-800 hover:bg-green-800 text-green-900 hover:text-white font-semibold flex-1 md:ml-2"
            onClick={loginAsDemoManager}
          >
            Login as Demo Manager
          </button>
        </div>
      </form>
    </div>
  );
};

const LoginPage: FC = () => {
  const router = useRouter();

  const { isLoggedIn, user } = useAuth();

  if (isLoggedIn) {
    if (router.query.redirect_to) {
      router.push(router.query.redirect_to.toString());
    } else if (user.role === "RestaurantManager") {
      router.push("/dashboard");
    } else {
      router.push("/");
    }
    return null;
  }

  return (
    <Layout title="Login">
      <Login />
    </Layout>
  );
};

export default LoginPage;
