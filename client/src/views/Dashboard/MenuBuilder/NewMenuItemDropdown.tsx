import React from "react";
import { useForm } from "react-hook-form";
import { MenuCategoryDto } from "~/api/restaurants/MenuDto";
import PlusIcon from "~/components/Icons/PlusIcon";
import { setFormErrors } from "~/services/forms/setFormErrors";
import { combineRules, MinRule, RequiredRule } from "~/services/forms/Rule";
import { ErrorAlert } from "~/components/Alert/Alert";
import useRestaurants from "~/store/restaurants/useRestaurants";
import { AddMenuItemRequest } from "~/api/restaurants/restaurantsApi";

interface FormValues {
  name: string;
  description: string;
  price: number;
}

interface Props {
  category: MenuCategoryDto;
}

const NewMenuItemDropdown: React.FC<Props> = ({ category }) => {
  const [isOpen, setIsOpen] = React.useState(false);
  const [error, setError] = React.useState(null);
  const restaurants = useRestaurants();

  const form = useForm<FormValues>({
    defaultValues: {
      name: "",
      description: "",
      price: null,
    },
    mode: "onBlur",
    reValidateMode: "onChange",
  });

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    setError(null);

    const request: AddMenuItemRequest = {
      category: category.name,
      ...data,
      price: +data.price,
    };

    const result = await restaurants.addMenuItem(request);

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

  React.useEffect(() => {
    form.register("name", {
      validate: combineRules([new RequiredRule()]),
    });
    form.register("description", {
      validate: combineRules([new RequiredRule()]),
    });
    form.register("price", {
      validate: combineRules([new RequiredRule(), new MinRule(0)]),
    });
  }, [form.register]);

  const handleCancel = () => {
    form.reset();
    setIsOpen(false);
  };

  return (
    <div className="rounded border-dashed border border-gray-400 mt-2">
      <button
        type="button"
        onClick={() => setIsOpen(true)}
        className="w-full px-4 py-3 text-gray-600 font-medium flex items-center hover:shadow-sm cursor-pointer hover:bg-gray-100 hover:text-gray-700"
      >
        <PlusIcon className="w-4 h-4" />
        <span className="ml-2">Add New Menu Item</span>
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
            <label className="label" htmlFor="description">
              Description <span className="text-primary">*</span>
            </label>
            <textarea
              ref={form.register}
              className="input"
              name="description"
              id="description"
              data-invalid={!!form.errors.description}
            ></textarea>
            {form.errors.description && (
              <p className="form-error mt-1">
                {form.errors.description.message}
              </p>
            )}
          </div>

          <div className="mt-4">
            <label className="label" htmlFor="price">
              Price <span className="text-primary">*</span>
            </label>
            <input
              ref={form.register}
              className="input"
              type="number"
              min="0"
              step="0.01"
              name="price"
              id="price"
              data-invalid={!!form.errors.price}
            />
            {form.errors.price && (
              <p className="form-error mt-1">{form.errors.price.message}</p>
            )}
          </div>

          <div className="mt-4">
            <button
              type="submit"
              disabled={form.formState.isSubmitting}
              className="btn-sm btn-primary"
            >
              Add Item
            </button>
            <button
              type="button"
              onClick={handleCancel}
              disabled={form.formState.isSubmitting}
              className="btn-sm btn-outline-primary ml-2"
            >
              Cancel
            </button>
          </div>
        </form>
      )}
    </div>
  );
};

export default NewMenuItemDropdown;
