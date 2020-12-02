import React from "react";
import { useForm } from "react-hook-form";
import { MenuCategoryDto } from "~/api/menu/MenuDto";
import useMenu from "~/api/menu/useMenu";
import useAddMenuItem from "~/api/restaurants/useAddMenuItem";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import PlusIcon from "~/components/Icons/PlusIcon";
import { combineRules, MinRule, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";

interface Props {
  category: MenuCategoryDto;
}

const NewMenuItemDropdown: React.FC<Props> = ({ category }) => {
  const { user } = useAuth();
  const { data: menu } = useMenu(user.restaurantId);

  const [isOpen, setIsOpen] = React.useState(false);

  const [addItem, { isError, error, reset }] = useAddMenuItem();

  const form = useForm({
    defaultValues: {
      itemName: "",
      description: "",
      price: null,
    },
  });

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    await addItem(
      {
        restaurantId: menu.restaurantId,
        categoryName: category.name,
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
              <ErrorAlert message={error.message} />
            </div>
          )}

          <div className="mt-4">
            <label className="label" htmlFor="itemName">
              Name <span className="text-primary">*</span>
            </label>
            <input
              ref={form.register({
                validate: combineRules([new RequiredRule()]),
              })}
              className="input"
              type="text"
              name="itemName"
              id="itemName"
              data-invalid={!!form.errors.itemName}
            />
            {form.errors.itemName && (
              <p className="form-error mt-1">{form.errors.itemName.message}</p>
            )}
          </div>

          <div className="mt-4">
            <label className="label" htmlFor="description">
              Description <span className="text-primary">*</span>
            </label>
            <textarea
              ref={form.register({
                validate: combineRules([new RequiredRule()]),
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
                validate: combineRules([new RequiredRule(), new MinRule(0)]),
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
