import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC } from "react";
import useAuth, { getAuthUser } from "~/api/users/useAuth";
import useRegister from "~/api/users/useRegister";
import { ErrorAlert } from "~/components/Alert/Alert";
import Layout from "~/components/Layout/Layout";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";

const Register: FC = () => {
  const router = useRouter();

  const { setUser } = useAuth();
  const { mutateAsync: register } = useRegister();

  const form = useForm({
    defaultValues: {
      firstName: "",
      lastName: "",
      email: "",
      password: "",
    },
  });

  const rules = useRules({
    firstName: (builder) => builder.required(),
    lastName: (builder) => builder.required(),
    email: (builder) => builder.required().email(),
    password: (builder) => builder.required().password(),
  });

  const onSubmit = form.handleSubmit(async (data) => {
    await register(data);
    setUser(await getAuthUser());
    await router.push("/");
  });

  return (
    <div className="mt-4 bg-white rounded shadow-sm p-4 max-w-3xl mx-auto">
      <h1 className="font-semibold text-2xl text-gray-800 text-center mt-2">
        Create account
      </h1>

      {form.error && !form.hasValidationErrors && (
        <div className="my-6">
          <ErrorAlert message={form.error.message} />
        </div>
      )}

      <form className="mt-4" onSubmit={onSubmit}>
        <div className="mt-4">
          <label className="label" htmlFor="firstName">
            First Name <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: rules.firstName,
            })}
            className="input"
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
            name="password"
            id="password"
            type="password"
            data-invalid={!!form.errors.password}
          />
          {form.errors.password && (
            <p className="form-error mt-1">{form.errors.password.message}</p>
          )}
        </div>

        <div className="mt-4">
          <button
            type="submit"
            disabled={isLoading}
            className="btn btn-primary font-semibold w-full normal-case"
          >
            Create account
          </button>
        </div>

        <div className="mt-4 text-center">
          <p className="text-sm text-gray-600">
            <span>Already have an account?</span>{" "}
            <Link href="/login">
              <a className="underline text-blue-700">Sign in</a>
            </Link>
          </p>
        </div>
      </form>
    </div>
  );
};

const RegisterPage: FC = () => {
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
    <Layout title="Register">
      <Register />
    </Layout>
  );
};

export default RegisterPage;
