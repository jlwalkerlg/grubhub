import React from "react";
import { useForm } from "react-hook-form";
import { AddMenuCategoryRequest } from "~/api/restaurants/restaurantsApi";
import { ErrorAlert } from "~/components/Alert/Alert";
import PlusIcon from "~/components/Icons/PlusIcon";
import { combineRules, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
import useRestaurants from "~/store/restaurants/useRestaurants";

interface FormValues {
  name: string;
}

const AddMenuCategoryForm: React.FC = () => {
  const restaurants = useRestaurants();

  const [isOpen, setIsOpen] = React.useState(false);
  const [error, setError] = React.useState(null);

  const form = useForm<FormValues>({
    defaultValues: {
      name: "",
    },
  });

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    setError(null);

    const request: AddMenuCategoryRequest = {
      ...data,
    };

    const result = await restaurants.addMenuCategory(request);

    if (!result.isSuccess) {
      setError(result.error.message);

      if (result.error.isValidationError) {
        setFormErrors(result.error.errors, form);
      }

      return;
    }

    setIsOpen(false);
    form.reset();
  });

  const handleCancel = () => {
    setIsOpen(false);
    form.reset();
    setError(null);
  };

  return (
    <div className="rounded border-dashed border border-gray-400 mt-2">
      <button
        type="button"
        onClick={() => setIsOpen(true)}
        className="w-full px-4 py-3 text-gray-600 font-medium flex items-center hover:shadow-sm cursor-pointer hover:bg-gray-100 hover:text-gray-700"
      >
        <PlusIcon className="w-4 h-4" />
        <span className="ml-2">Add Menu Category</span>
      </button>

      {isOpen && (
        <form onSubmit={onSubmit} className="px-4 pb-3">
          {error && (
            <div className="my-3">
              <ErrorAlert message={error} />
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
            <button
              type="submit"
              disabled={form.formState.isSubmitting}
              className="btn btn-sm btn-primary"
            >
              Add Category
            </button>
            <button
              type="button"
              onClick={handleCancel}
              disabled={form.formState.isSubmitting}
              className="btn btn-sm btn-outline-primary ml-2"
            >
              Cancel
            </button>
          </div>
        </form>
      )}
    </div>
  );
};

export default AddMenuCategoryForm;
