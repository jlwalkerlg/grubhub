import React, { FC, useState } from "react";
import useCuisines from "~/api/cuisines/useCuisines";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useUpdateCuisines from "~/api/restaurants/useUpdateCuisines";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import CheckIcon from "~/components/Icons/CheckIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { DashboardLayout } from "../DashboardLayout";
import styles from "./UpdateCuisines.module.css";

const UpdateCuisinesForm: FC = () => {
  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);
  const { data: cuisines } = useCuisines();

  const [selected, setSelected] = useState(() =>
    restaurant.cuisines.map((x) => x.name)
  );

  const [
    updateCuisines,
    { isError, error, isSuccess, isLoading },
  ] = useUpdateCuisines(restaurant.id);

  const onSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    updateCuisines({ cuisines: selected });
  };

  const onClick = (cuisine: string) => {
    if (selected.includes(cuisine)) {
      setSelected(selected.filter((x) => x !== cuisine));
    } else {
      setSelected([...selected, cuisine]);
    }
  };

  return (
    <form onSubmit={onSubmit}>
      {isError && (
        <div className="my-6">
          <ErrorAlert message={error.message} />
        </div>
      )}

      {isSuccess && (
        <div className="my-6">
          <SuccessAlert message="Cuisines updated!" />
        </div>
      )}

      <ul className="mt-3">
        {cuisines.map((cuisine) => {
          const isSelected = selected.includes(cuisine.name);

          return (
            <li key={cuisine.name} className="mt-2">
              <button
                key={cuisine.name}
                type="button"
                onClick={() => onClick(cuisine.name)}
                className={`flex items-center w-full py-2 px-3 mt-3 bg-white rounded-lg border border-gray-300 hover:border-gray-400 ${
                  styles["cuisine-link"]
                } ${isSelected ? styles["selected"] : ""}`}
              >
                <CheckIcon className={`w-5 h-5 ${styles["cuisine-check"]}`} />
                <span className={styles["cuisine-name"]}>{cuisine.name}</span>
              </button>
            </li>
          );
        })}
      </ul>

      <div className="mt-4">
        <button
          type="submit"
          disabled={isLoading}
          className="btn btn-primary font-semibold w-full"
        >
          Update
        </button>
      </div>
    </form>
  );
};

const UpdateCuisines: FC = () => {
  const { isLoggedIn, user } = useAuth();

  const {
    isLoading: isLoadingRestaurant,
    isError: isRestaurantError,
    error: restaurantsError,
  } = useRestaurant(user?.restaurantId, { enabled: isLoggedIn });

  const {
    isLoading: isLoadingCuisines,
    isError: isCuisinesError,
    error: cuisinesError,
  } = useCuisines();

  if (isLoadingRestaurant || isLoadingCuisines) {
    return (
      <DashboardLayout>
        <SpinnerIcon className="w-6 h-6" />
      </DashboardLayout>
    );
  }

  if (isRestaurantError || isCuisinesError) {
    return (
      <DashboardLayout>
        <p>
          Failed to load cuisines: {(restaurantsError || cuisinesError).message}
        </p>
      </DashboardLayout>
    );
  }

  return (
    <DashboardLayout>
      <h2 className="text-xl lg:text-2xl font-semibold text-gray-800 tracking-wider">
        Update Cuisines
      </h2>

      {isLoggedIn && <UpdateCuisinesForm />}
    </DashboardLayout>
  );
};

export default UpdateCuisines;
