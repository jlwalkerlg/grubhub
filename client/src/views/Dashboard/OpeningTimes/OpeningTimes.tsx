import { flatten, padStart, range } from "lodash";
import { NextPage } from "next";
import React from "react";
import { useForm, UseFormMethods } from "react-hook-form";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useUpdateOpeningTimes, {
  UpdateOpeningTimesCommand,
} from "~/api/restaurants/useUpdateOpeningTimes";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { setFormErrors } from "~/services/forms/setFormErrors";
import { DashboardLayout } from "../DashboardLayout";

const openingTimes = flatten(
  range(0, 24).map((hr) =>
    range(0, 60, 15).map(
      (min) =>
        `${padStart(hr.toString(), 2, "0")}:${padStart(min.toString(), 2, "0")}`
    )
  )
);

const closingTimes = openingTimes.slice(1);

const OpeningTimesInput: React.FC<{
  form: UseFormMethods<UpdateOpeningTimesCommand>;
  day: string;
  openingName: keyof UpdateOpeningTimesCommand;
  closingName: keyof UpdateOpeningTimesCommand;
}> = ({ form, day, openingName, closingName }) => {
  const close = () => {
    form.setValue(openingName, "");
    form.setValue(closingName, "");
  };

  const open = () => {
    form.setValue(openingName, "00:00");
    form.setValue(closingName, "");
  };

  const changeOpeningTime = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const time = e.target.value;

    if (time === "23:45") {
      form.setValue(closingName, "");
    }

    const index = openingTimes.indexOf(time);
    if (
      index > Math.max(0, closingTimes.indexOf(form.getValues(closingName)))
    ) {
      form.setValue(closingName, closingTimes[index]);
    }
  };

  const changeClosingTime = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const time = e.target.value;

    if (!time) return;

    const index = closingTimes.indexOf(time);
    if (index < openingTimes.indexOf(form.getValues(openingName))) {
      form.setValue(openingName, openingTimes[index]);
    }
  };

  const isOpen = form.watch(openingName) !== "";

  return (
    <div className="mt-3 text-center">
      <div className="lg:flex items-center justify-center">
        <label
          className="label lg:w-1/5 lg:text-left text-base text-gray-800 tracking-wide"
          htmlFor={openingName}
        >
          {day}
        </label>

        <div className="mt-1 mx-auto flex justify-center items-center">
          {!isOpen && <div className="italic tracking-wide">Closed</div>}

          <div className={isOpen ? undefined : "hidden"}>
            <div>
              <select
                ref={form.register}
                name={openingName}
                id={openingName}
                onChange={changeOpeningTime}
              >
                {openingTimes.map((time) => (
                  <option key={time} value={time}>
                    {time}
                  </option>
                ))}
              </select>
              <span>&mdash;</span>
              <select
                ref={form.register}
                name={closingName}
                id={closingName}
                onChange={changeClosingTime}
              >
                {closingTimes.map((time) => (
                  <option key={time} value={time}>
                    {time}
                  </option>
                ))}
                <option value="">Midnight</option>
              </select>
            </div>
          </div>
        </div>

        <div className="lg:w-1/5 lg:text-right mt-1">
          {isOpen ? (
            <button type="button" onClick={close} className="text-red-700">
              Close
            </button>
          ) : (
            <button type="button" onClick={open} className="text-red-700">
              Open
            </button>
          )}
        </div>
      </div>

      {(form.errors[openingName] || form.errors[closingName]) && (
        <p className="form-error mt-1 text-center">
          {(form.errors[openingName] || form.errors[closingName]).message}
        </p>
      )}
    </div>
  );
};

const UpdateOpeningTimesForm: React.FC<{ restaurant: RestaurantDto }> = ({
  restaurant,
}) => {
  const [
    updateOpeningTimes,
    { isError, error, isSuccess },
  ] = useUpdateOpeningTimes(restaurant.id);

  const form = useForm<UpdateOpeningTimesCommand>({
    defaultValues: {
      mondayOpen: restaurant.openingTimes.monday?.open || "",
      mondayClose: restaurant.openingTimes.monday?.close || "",
      tuesdayOpen: restaurant.openingTimes.tuesday?.open || "",
      tuesdayClose: restaurant.openingTimes.tuesday?.close || "",
      wednesdayOpen: restaurant.openingTimes.wednesday?.open || "",
      wednesdayClose: restaurant.openingTimes.wednesday?.close || "",
      thursdayOpen: restaurant.openingTimes.thursday?.open || "",
      thursdayClose: restaurant.openingTimes.thursday?.close || "",
      fridayOpen: restaurant.openingTimes.friday?.open || "",
      fridayClose: restaurant.openingTimes.friday?.close || "",
      saturdayOpen: restaurant.openingTimes.saturday?.open || "",
      saturdayClose: restaurant.openingTimes.saturday?.close || "",
      sundayOpen: restaurant.openingTimes.sunday?.open || "",
      sundayClose: restaurant.openingTimes.sunday?.close || "",
    },
  });

  const onSubmit = form.handleSubmit(async (command) => {
    if (form.formState.isSubmitting) return;

    await updateOpeningTimes(command, {
      onError: (error) => {
        if (error.isValidationError) {
          setFormErrors(error.errors, form);
        }
      },
    });
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
          <SuccessAlert message="Opening times updated!" />
        </div>
      )}

      <div className="mt-4">
        <OpeningTimesInput
          form={form}
          day="Monday"
          openingName="mondayOpen"
          closingName="mondayClose"
        />
        <OpeningTimesInput
          form={form}
          day="Tuesday"
          openingName="tuesdayOpen"
          closingName="tuesdayClose"
        />
        <OpeningTimesInput
          form={form}
          day="Wednesday"
          openingName="wednesdayOpen"
          closingName="wednesdayClose"
        />
        <OpeningTimesInput
          form={form}
          day="Thursday"
          openingName="thursdayOpen"
          closingName="thursdayClose"
        />
        <OpeningTimesInput
          form={form}
          day="Friday"
          openingName="fridayOpen"
          closingName="fridayClose"
        />
        <OpeningTimesInput
          form={form}
          day="Saturday"
          openingName="saturdayOpen"
          closingName="saturdayClose"
        />
        <OpeningTimesInput
          form={form}
          day="Sunday"
          openingName="sundayOpen"
          closingName="sundayClose"
        />
      </div>

      <div className="mt-8">
        <button
          type="submit"
          className="btn btn-primary font-semibold w-full"
          disabled={form.formState.isSubmitting}
        >
          Update
        </button>
      </div>
    </form>
  );
};

const OpeningTimes: NextPage = () => {
  const { isLoggedIn, user } = useAuth();

  const {
    data: restaurant,
    isLoading,
    isFetching,
    isError,
  } = useRestaurant(user?.restaurantId, { enabled: isLoggedIn });

  return (
    <DashboardLayout>
      <div className="flex items-center justify-center lg:justify-start">
        <h2 className="text-xl lg:text-2xl font-semibold text-gray-800 tracking-wider">
          Opening Times
        </h2>
        {!isLoading && isFetching && (
          <div className="ml-2">
            <SpinnerIcon className="w-6 h-6 animate-spin" />
          </div>
        )}
      </div>

      {isError && <p>Failed to load restaurant.</p>}
      {isLoading && (
        <div className="mt-2">
          <SpinnerIcon className="w-5 h-5 animate-spin" />
        </div>
      )}
      {isLoggedIn && !isLoading && !isError && (
        <UpdateOpeningTimesForm restaurant={restaurant} />
      )}
    </DashboardLayout>
  );
};

export default OpeningTimes;
