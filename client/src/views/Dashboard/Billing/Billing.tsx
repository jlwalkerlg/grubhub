import React, { FC } from "react";
import useBillingDetails from "~/api/billing/useBillingDetails";
import useSetupBilling from "~/api/billing/useSetupBilling";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import { DashboardLayout } from "../DashboardLayout";

const Billing: FC = () => {
  const { addToast } = useToasts();

  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);
  const {
    data: billingDetails,
    isLoading,
    isError,
  } = useBillingDetails(restaurant.id);

  const { mutate: setupBilling, isLoading: isSettingUpBilling } =
    useSetupBilling();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError) {
    return <div>Billing details could not be loaded at this time.</div>;
  }

  const onSetupBilling = () => {
    if (isSettingUpBilling) return;

    setupBilling(restaurant.id, {
      onSuccess: (link) => {
        window.location.href = link;
      },

      onError: (error) => {
        addToast(error.message);
      },
    });
  };

  return (
    <div>
      {billingDetails?.enabled ? (
        <div>
          Billing is enabled. Need to{" "}
          <button
            onClick={onSetupBilling}
            className="btn btn-link"
            disabled={isSettingUpBilling}
          >
            update your billing details
          </button>
          ?
        </div>
      ) : (
        <div>
          Billing is currently disabled. Please{" "}
          <button
            onClick={onSetupBilling}
            className="btn btn-link"
            disabled={isSettingUpBilling}
          >
            setup billing
          </button>{" "}
          to activate your restaurant and start collecting orders.
        </div>
      )}
    </div>
  );
};

const BillingLayout: FC = () => {
  const { isLoading: isLoadingUser, isLoggedIn, user } = useAuth();

  const { isLoading: isLoadingRestaurant, isError } = useRestaurant(
    user?.restaurantId,
    {
      enabled: isLoggedIn,
    }
  );

  const isLoading = isLoadingUser || isLoadingRestaurant;

  return (
    <DashboardLayout>
      <h2 className="text-xl lg:text-2xl font-semibold text-gray-800 tracking-wider">
        Billing
      </h2>

      {isLoggedIn && (
        <div className="mt-2">
          {isLoading && <SpinnerIcon className="w-6 h-6 animate-spin" />}

          {!isLoading && isError && (
            <p>Restaurant could not be loaded at this time.</p>
          )}

          {!isLoading && !isError && <Billing />}
        </div>
      )}
    </DashboardLayout>
  );
};

export default BillingLayout;
