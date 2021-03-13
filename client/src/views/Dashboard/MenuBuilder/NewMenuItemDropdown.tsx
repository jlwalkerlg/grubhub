import React from "react";
import { useForm } from "react-hook-form";
import useAddMenuItem from "~/api/menu/useAddMenuItem";
import useRestaurant, {
  MenuCategoryDto,
} from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import PlusIcon from "~/components/Icons/PlusIcon";
import { useRules } from "~/services/useRules";
import { setFormErrors } from "~/services/utils";

const NewMenuItemDropdown: React.FC<{
  category: MenuCategoryDto;
}> = ({ category }) => {
  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);

  const [isOpen, setIsOpen] = React.useState(false);

  const [addItem, { isError, error, reset }] = useAddMenuItem();

  const form = useForm({
    defaultValues: {
      name: "",
      description: "",
      price: null,
    },
  });

  const rules = useRules({
    name: (builder) => builder.required(),
    description: (builder) => builder.required().maxLength(280),
    price: (builder) => builder.required().min(0),
  });

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    await addItem(
      {
        restaurantId: restaurant.id,
        categoryId: category.id,
        ...data,
        price: +data.price,
      },
      {
        onSuccess: () => {
          setIsOpen(false);
          form.reset();
        },
        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, form);
          }
        },
      }
    );
  });

  const handleCancel = () => {
    setIsOpen(false);
    form.reset();
    reset();
  };

  return (
    <div className="rounded border-dashed border border-gray-400 mt-2">
      <button
        type="button"
        onClick={() => setIsOpen(true)}
        className="w-full px-4 py-3 text-gray-600 font-medium flex items-center hover:shadow-sm cursor-pointer hover:bg-gray-100 hover:text-gray-700"
      >
        <PlusIcon className="w-4 h-4" />
        <span className="ml-2">Add Menu Item</span>
      </button>

      {isOpen && (
        <form onSubmit={onSubmit} className="px-4 pb-3">
          {isError && (
            <div className="my-3">
              <ErrorAlert message={error.detail} />
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
              ref={form.register({
                validate: rules.price,
              })}
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
              className="w-full lg:w-auto btn btn-sm btn-primary"
            >
              Add Item
            </button>
            <button
              type="button"
              onClick={handleCancel}
              disabled={form.formState.isSubmitting}
              className="w-full lg:w-auto btn btn-sm btn-outline-primary mt-3 lg:mt-0 lg:ml-2"
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
