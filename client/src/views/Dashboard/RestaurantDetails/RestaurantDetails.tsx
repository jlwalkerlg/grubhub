import { NextPage } from "next";
import React, { FC } from "react";
import useRestaurant, { RestaurantDto } from "~/api/restaurants/useRestaurant";
import useUpdateRestaurantDetails from "~/api/restaurants/useUpdateRestaurantDetails";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";
import { DashboardLayout } from "../DashboardLayout";

const RestaurantDetailsForm: FC<{ restaurant: RestaurantDto }> = ({
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
    name: (b) => b.required(),
    description: (b) => b.required().maxLength(280),
    phoneNumber: (b) => b.required().phone(),
    deliveryFee: (b) => b.required().min(0),
    minimumDeliverySpend: (b) => b.required().min(0),
    maxDeliveryDistanceInKm: (b) => b.required().min(0),
    estimatedDeliveryTimeInMinutes: (b) => b.required().min(1),
  });

  const { mutateAsync: updateRestaurantDetails } = useUpdateRestaurantDetails();

  const onSubmit = form.handleSubmit(async (data) => {
    await updateRestaurantDetails({
      id: restaurant.id,
      ...data,
    });
  });

  return (
    <form onSubmit={onSubmit}>
      {form.error && (
        <div className="my-6">
          <ErrorAlert message={form.error.message} />
        </div>
      )}

      {form.isSuccess && (
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
          disabled={form.isLoading}
          className="btn btn-primary font-semibold w-full"
        >
          Update
        </button>
      </div>
    </form>
  );
};

const RestaurantDetails: FC = () => {
  const { user } = useAuth();

  const { data: restaurant, isLoading, isError } = useRestaurant(
    user.restaurantId
  );

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
