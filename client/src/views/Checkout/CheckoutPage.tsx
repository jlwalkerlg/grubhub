import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC } from "react";
import { useQueryClient } from "react-query";
import useBasket from "~/api/baskets/useBasket";
import { getOrder, getOrderQueryKey } from "~/api/orders/useOrder";
import { usePlaceOrder } from "~/api/orders/usePlaceOrder";
import useRestaurant, { RestaurantDto } from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { AuthLayout } from "~/components/Layout/Layout";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";

const CheckoutForm: FC<{ restaurant: RestaurantDto }> = ({ restaurant }) => {
  const router = useRouter();

  const { user } = useAuth();

  const queryClient = useQueryClient();

  const { mutateAsync: placeOrder } = usePlaceOrder();

  const form = useForm({
    defaultValues: {
      mobile: user.mobileNumber,
      addressLine1: user.addressLine1,
      addressLine2: user.addressLine2,
      city: user.city,
      postcode: user.postcode,
    },
  });

  const rules = useRules({
    mobile: (b) => b.required().mobile(),
    addressLine1: (b) => b.required(),
    city: (b) => b.required(),
    postcode: (b) => b.required().postcode(),
  });

  const handleSubmit = form.handleSubmit(async (data) => {
    const orderId = await placeOrder({
      restaurantId: restaurant.id,
      ...data,
    });
    queryClient.prefetchQuery(getOrderQueryKey(orderId), () =>
      getOrder(orderId)
    );
    await router.push(`/orders/${orderId}/pay`);
  });

  return (
    <div>
      <h2 className="font-bold text-xl text-gray-800 text-center pb-2">
        {user.firstName}, confirm your details.
      </h2>

      {form.error && (
        <p className="text-primary text-center mt-2">{form.error.message}</p>
      )}

      <form onSubmit={handleSubmit} className="mt-4">
        <div>
          <label className="label" htmlFor="mobile">
            Mobile Number <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: rules.mobile,
            })}
            className="input"
            type="text"
            name="mobile"
            id="mobile"
            data-invalid={!!form.errors.mobile}
          />
          {form.errors.mobile && (
            <p className="form-error mt-1">{form.errors.mobile.message}</p>
          )}
        </div>

        <div className="mt-6">
          <label className="label" htmlFor="addressLine1">
            Address Line 1 <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: rules.addressLine1,
            })}
            className="input"
            type="text"
            name="addressLine1"
            id="addressLine1"
            data-invalid={!!form.errors.addressLine1}
          />
          {form.errors.addressLine1 && (
            <p className="form-error mt-1">
              {form.errors.addressLine1.message}
            </p>
          )}
        </div>

        <div className="mt-3">
          <label className="label" htmlFor="addressLine2">
            Address Line 2 (optional)
          </label>
          <input
            ref={form.register}
            className="input"
            type="text"
            name="addressLine2"
            id="addressLine2"
            data-invalid={!!form.errors.addressLine2}
          />
          {form.errors.addressLine2 && (
            <p className="form-error mt-1">
              {form.errors.addressLine2.message}
            </p>
          )}
        </div>

        <div className="mt-3">
          <label className="label" htmlFor="city">
            City <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: rules.city,
            })}
            className="input"
            type="text"
            name="city"
            id="city"
            data-invalid={!!form.errors.city}
          />
          {form.errors.city && (
            <p className="form-error mt-1">{form.errors.city.message}</p>
          )}
        </div>

        <div className="mt-3">
          <label className="label" htmlFor="postcode">
            Postcode <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: rules.postcode,
            })}
            className="input"
            type="text"
            name="postcode"
            id="postcode"
            data-invalid={!!form.errors.postcode}
          />
          {form.errors.postcode && (
            <p className="form-error mt-1">{form.errors.postcode.message}</p>
          )}
        </div>

        <button
          className="btn btn-primary w-full mt-6"
          disabled={form.isLoading}
        >
          Go to payment
        </button>
      </form>
    </div>
  );
};

const Checkout: FC = () => {
  const router = useRouter();
  const restaurantId = router.query.id?.toString();

  const {
    data: restaurant,
    isLoading: isLoadingRestaurant,
    isError: isRestaurantError,
  } = useRestaurant(restaurantId, {
    enabled: router.isReady,
  });

  const {
    data: basket,
    isLoading: isLoadingBasket,
    isError: isBasketError,
  } = useBasket(restaurantId, {
    enabled: router.isReady,
  });

  const isLoading = isLoadingRestaurant || isLoadingBasket || !router.isReady;
  const isError = isRestaurantError || isBasketError;

  if (isLoading) {
    return (
      <div className="flex flex-col justify-center items-center">
        <SpinnerIcon className="w-6 h-6 animate-spin" />
        <p className="mt-2 text-gray-700">Loading order.</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="flex flex-col justify-center items-center">
        <p>
          There was an error loading your order. Return to{" "}
          <Link href={`/restaurants/${restaurantId}`}>
            <a className="text-primary cursor-pointer">restaurant</a>
          </Link>{" "}
          and try again.
        </p>
      </div>
    );
  }

  const subtotal =
    basket?.items.reduce(
      (acc, item) => acc + item.menuItemPrice * item.quantity,
      0
    ) ?? 0;

  if (subtotal < restaurant.minimumDeliverySpend) {
    router.push(`/restaurants/${restaurantId}`);
    return null;
  }

  return <CheckoutForm restaurant={restaurant} />;
};

const CheckoutPage: FC = () => {
  return (
    <AuthLayout title="Grub Hub | Checkout">
      <div className="container max-w-xl mt-4">
        <div className="rounded-sm bg-white border border-gray-200 py-6 px-8">
          <Checkout />
        </div>
      </div>
    </AuthLayout>
  );
};

export default CheckoutPage;
