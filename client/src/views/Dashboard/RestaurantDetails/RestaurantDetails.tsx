import { NextPage } from "next";
import React from "react";
import { useForm } from "react-hook-form";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useUpdateRestaurantDetails from "~/api/restaurants/useUpdateRestaurantDetails";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { combineRules, PhoneRule, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
import { DashboardLayout } from "../DashboardLayout";

const RestaurantDetailsForm: React.FC<{ restaurant: RestaurantDto }> = ({
  restaurant,
}) => {
  const form = useForm({
    defaultValues: {
      name: restaurant.name,
      phoneNumber: restaurant.phoneNumber,
    },
  });

  const [
    updateRestaurantDetails,
    { isError, error, isSuccess },
  ] = useUpdateRestaurantDetails();

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    await updateRestaurantDetails(
      {
        id: restaurant.id,
        ...data,
      },
      {
        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, form);
          }
        },
      }
    );
  });

  return (
    <form onSubmit={onSubmit}>
      {isError && (
        <div className="my-6">
          <ErrorAlert message={error.message} />
        </div>
      )}

      {isSuccess && (
        <div className="my-6">
          <SuccessAlert message="Restaurant details updated!" />
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
        <label className="label" htmlFor="phoneNumber">
          Phone Number <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: combineRules([new RequiredRule(), new PhoneRule()]),
          })}
          className="input"
          type="text"
          name="phoneNumber"
          id="phoneNumber"
          data-invalid={!!form.errors.phoneNumber}
        />
        {form.errors.phoneNumber && (
          <p className="form-error mt-1">{form.errors.phoneNumber.message}</p>
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

const RestaurantDetails: React.FC = () => {
  const { user } = useAuth();

  const {
    data: restaurant,
    isLoading,
    isError,
    error: loadingError,
  } = useRestaurant(user.restaurantId);

  if (isLoading) {
    return <SpinnerIcon className="w-6 h-6 animate-spin" />;
  }

  if (isError) {
    return (
      <ErrorAlert
        message={`Failed to load restaurant: ${loadingError.message}`}
      />
    );
  }

  return <RestaurantDetailsForm restaurant={restaurant} />;
};

const RestaurantDetailsWrapper: NextPage = () => {
  const { isLoggedIn } = useAuth();

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Restaurant Details
      </h2>

      {isLoggedIn && <RestaurantDetails />}
    </DashboardLayout>
  );
};

export default RestaurantDetailsWrapper;
