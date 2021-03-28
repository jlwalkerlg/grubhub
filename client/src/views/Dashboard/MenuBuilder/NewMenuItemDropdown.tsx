import React, { FC, useState } from "react";
import useAddMenuItem from "~/api/menu/useAddMenuItem";
import useRestaurant, {
  MenuCategoryDto,
} from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import PlusIcon from "~/components/Icons/PlusIcon";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";

const NewMenuItemDropdown: FC<{
  category: MenuCategoryDto;
}> = ({ category }) => {
  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);

  const [isOpen, setIsOpen] = useState(false);

  const { mutateAsync: addItem, reset } = useAddMenuItem();

  const form = useForm({
    defaultValues: {
      name: "",
      description: "",
      price: null,
    },
  });

  const rules = useRules({
    name: (b) => b.required(),
    description: (b) => b.required().maxLength(280),
    price: (b) => b.required().min(0),
  });

  const onSubmit = form.handleSubmit(async (data) => {
    await addItem({
      restaurantId: restaurant.id,
      categoryId: category.id,
      ...data,
      price: +data.price,
    });

    setIsOpen(false);
    form.reset();
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
          {form.error && (
            <div className="my-3">
              <ErrorAlert message={form.error.message} />
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
              disabled={form.isLoading}
              className="w-full lg:w-auto btn btn-sm btn-primary"
            >
              Add Item
            </button>
            <button
              type="button"
              onClick={handleCancel}
              disabled={form.isLoading}
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
