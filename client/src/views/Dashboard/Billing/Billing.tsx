import React, { FC } from "react";
import useBillingDetails from "~/api/billing/useBillingDetails";
import useGenerateOnboardingLink from "~/api/billing/useGenerateOnboardingLink";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import { DashboardLayout } from "../DashboardLayout";

const Billing: FC = () => {
  const { addToast } = useToasts();

  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);
  const { data: billingDetails, isLoading, isError, error } = useBillingDetails(
    restaurant.id
  );

  const {
    refetch: generateOnboardingLink,
    isFetching: isLoadingLink,
  } = useGenerateOnboardingLink(restaurant.id, {
    onSuccess: (link) => {
      window.location.href = link;
    },
    onError: (error) => {
      addToast(error.message);
    },
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError) {
    return <div>{error.message}</div>;
  }

  return (
    <div>
      {billingDetails.isBillingEnabled ? (
        <div>
          Billing is enabled. Need to{" "}
          <button
            onClick={() => generateOnboardingLink()}
            className="btn btn-link"
            disabled={isLoadingLink}
          >
            update your billing details
          </button>
          ?
        </div>
      ) : (
        <div>
          Please{" "}
          <button
            onClick={() => generateOnboardingLink()}
            className="btn btn-link"
            disabled={isLoadingLink}
          >
            complete onboarding
          </button>{" "}
          to setup billing. Your restaurant will not show up in search results
          until billing is enabled.
        </div>
      )}
    </div>
  );
};

const BillingLayout: FC = () => {
  const { isLoading: isLoadingUser, isLoggedIn, user } = useAuth();

  const { isLoading: isLoadingRestaurant, isError, error } = useRestaurant(
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

          {!isLoading && isError && <p>{error.message}</p>}

          {!isLoading && !isError && <Billing />}
        </div>
      )}
    </DashboardLayout>
  );
};

export default BillingLayout;
