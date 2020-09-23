import React from "react";
import { useForm } from "react-hook-form";
import { UpdateRestaurantDetailsCommand } from "~/api/restaurants/restaurantsApi";
import { ErrorAlert } from "~/components/Alert/Alert";
import { combineRules, PhoneRule, RequiredRule } from "~/lib/Form/Rule";
import useAuth from "~/store/auth/useAuth";
import { setFormErrors } from "~/utils/setFormErrors";

const UpdateRestaurantDetailsForm: React.FC = () => {
  const auth = useAuth();
  const [error, setError] = React.useState<string>(null);

  const form = useForm<UpdateRestaurantDetailsCommand>({
    defaultValues: {
      name: auth.restaurant.name,
      phoneNumber: auth.restaurant.phoneNumber,
    },
    mode: "onBlur",
    reValidateMode: "onChange",
  });

  React.useEffect(() => {
    form.register("name", {
      validate: combineRules([new RequiredRule()]),
    });
    form.register("phoneNumber", {
      validate: combineRules([new RequiredRule(), new PhoneRule()]),
    });
  }, [form.register]);

  const onSubmit = async (command: UpdateRestaurantDetailsCommand) => {
    if (form.formState.isSubmitting) return;

    setError(null);

    const result = await auth.updateRestaurantDetails(command);

    if (!result.isSuccess) {
      setError(result.error.message);

      if (result.error.isValidationError) {
        setFormErrors(result.error.errors, form);
      }

      return;
    }
  };

  return (
    <form onSubmit={form.handleSubmit(onSubmit)}>
      {error && (
        <div className="my-6">
          <ErrorAlert message={error} />
        </div>
      )}

      <div className="mt-4">
        <label className="label" htmlFor="name">
          Name <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register}
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
          ref={form.register}
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
          Submit
        </button>
      </div>
    </form>
  );
};

export default UpdateRestaurantDetailsForm;
