import { NextPage } from "next";
import React from "react";
import { useForm } from "react-hook-form";
import useRestaurant, { RestaurantDto } from "~/api/restaurants/useRestaurant";
import useUpdateRestaurantDetails from "~/api/restaurants/useUpdateRestaurantDetails";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useRules } from "~/services/useRules";
import { setFormErrors } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";

const RestaurantDetailsForm: React.FC<{ restaurant: RestaurantDto }> = ({
  restaurant,
}) => {
  const form = useForm({
    defaultValues: {
      name: restaurant.name,
      description: restaurant.description,
      phoneNumber: restaurant.phoneNumber,
      deliveryFee: restaurant.deliveryFee,
      minimumDeliverySpend: restaurant.minimumDeliverySpend,
      maxDeliveryDistanceInKm: restaurant.maxDeliveryDistanceInKm,
      estimatedDeliveryTimeInMinutes: restaurant.estimatedDeliveryTimeInMinutes,
    },
  });

  const rules = useRules({
    name: (builder) => builder.required(),
    description: (builder) => builder.required().maxLength(280),
    phoneNumber: (builder) => builder.required().phone(),
    deliveryFee: (builder) => builder.required().min(0),
    minimumDeliverySpend: (builder) => builder.required().min(0),
    maxDeliveryDistanceInKm: (builder) => builder.required().min(0),
    estimatedDeliveryTimeInMinutes: (builder) => builder.required().min(1),
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
          <ErrorAlert message={error.detail} />
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
            validate: rules.name,
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
        <label className="label" htmlFor="description">
          Description
        </label>
        <textarea
          ref={form.register({
            validate: rules.description,
          })}
          className="input"
          name="description"
          id="description"
          data-invalid={!!form.errors.description}
        ></textarea>
        {form.errors.description && (
          <p className="form-error mt-1">{form.errors.description.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="phoneNumber">
          Phone Number <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.phoneNumber,
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
        <label className="label" htmlFor="deliveryFee">
          Delivery Fee <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.deliveryFee,
          })}
          className="input"
          type="number"
          min="0"
          step="0.01"
          name="deliveryFee"
          id="deliveryFee"
          data-invalid={!!form.errors.deliveryFee}
        />
        {form.errors.deliveryFee && (
          <p className="form-error mt-1">{form.errors.deliveryFee.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="minimumDeliverySpend">
          Minimum Delivery Spend <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.minimumDeliverySpend,
          })}
          className="input"
          type="number"
          min="0"
          step="0.01"
          name="minimumDeliverySpend"
          id="minimumDeliverySpend"
          data-invalid={!!form.errors.minimumDeliverySpend}
        />
        {form.errors.minimumDeliverySpend && (
          <p className="form-error mt-1">
            {form.errors.minimumDeliverySpend.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="maxDeliveryDistanceInKm">
          Max Delivery Distance (km) <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.maxDeliveryDistanceInKm,
          })}
          className="input"
          type="number"
          min="0"
          step="1"
          name="maxDeliveryDistanceInKm"
          id="maxDeliveryDistanceInKm"
          data-invalid={!!form.errors.maxDeliveryDistanceInKm}
        />
        {form.errors.maxDeliveryDistanceInKm && (
          <p className="form-error mt-1">
            {form.errors.maxDeliveryDistanceInKm.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="estimatedDeliveryTimeInMinutes">
          Estimated Delivery Time (mins) <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.estimatedDeliveryTimeInMinutes,
          })}
          className="input"
          type="number"
          min="5"
          step="5"
          name="estimatedDeliveryTimeInMinutes"
          id="estimatedDeliveryTimeInMinutes"
          data-invalid={!!form.errors.estimatedDeliveryTimeInMinutes}
        />
        {form.errors.estimatedDeliveryTimeInMinutes && (
          <p className="form-error mt-1">
            {form.errors.estimatedDeliveryTimeInMinutes.message}
          </p>
        )}
      </div>

      <div className="mt-5">
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
      <ErrorAlert message="Restaurant could not be loaded at this time." />
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
