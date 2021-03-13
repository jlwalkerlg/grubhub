import React, { FC } from "react";
import { useForm } from "react-hook-form";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { MenuCategoryDto, MenuDto, MenuItemDto } from "~/api/menu/MenuDto";
import useRemoveMenuItem from "~/api/menu/useRemoveMenuItem";
import useUpdateMenuItem from "~/api/menu/useUpdateMenuItem";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import { setFormErrors } from "~/services/forms/setFormErrors";
import { useRules } from "~/services/forms/useRules";

const MySwal = withReactContent(Swal);

const UpdateForm: FC<{
  menu: MenuDto;
  category: MenuCategoryDto;
  item: MenuItemDto;
  close: () => any;
}> = ({ menu, category, item, close }) => {
  const [update, { isError, error, reset }] = useUpdateMenuItem();

  const form = useForm({
    defaultValues: {
      name: item.name,
      description: item.description,
      price: item.price,
    },
  });

  const rules = useRules(() => ({
    name: (builder) => builder.required(),
    description: (builder) => builder.required().maxLength(280),
    price: (builder) => builder.required().min(0),
  }));

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    await update(
      {
        restaurantId: menu.restaurantId,
        categoryId: category.id,
        itemId: item.id,
        ...data,
        price: +data.price,
      },
      {
        onSuccess: () => {
          form.setValue("name", data.name);
          form.setValue("description", data.description);
          form.setValue("price", data.price);
          close();
        },
        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, form);
          }
        },
      }
    );
  });

  const cancel = () => {
    close();
    form.reset();
    reset();
  };

  return (
    <form onSubmit={onSubmit} className="px-2 pb-3">
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
          <p className="form-error mt-1">{form.errors.description.message}</p>
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
          Update Item
        </button>
        <button
          type="button"
          onClick={cancel}
          disabled={form.formState.isSubmitting}
          className="w-full lg:w-auto btn btn-sm btn-outline-primary mt-3 lg:mt-0 lg:ml-2"
        >
          Cancel
        </button>
      </div>
    </form>
  );
};

const MenuItem: React.FC<{
  category: MenuCategoryDto;
  item: MenuItemDto;
}> = ({ category, item }) => {
  const { addToast } = useToasts();

  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);

  const [isUpdateFormOpen, setIsUpdateFormOpen] = React.useState(false);

  const [remove, removeStatus] = useRemoveMenuItem();

  const onRemove = async () => {
    if (removeStatus.isLoading) return;

    const confirmation = await MySwal.fire({
      title: (
        <p>
          Delete menu item {item.name} from category {category.name}?
        </p>
      ),
      icon: "warning",
      confirmButtonColor: "#c53030",
      confirmButtonText: "Delete",
      showCancelButton: true,
      cancelButtonColor: "#4a5568",
    });

    if (!confirmation.isConfirmed) {
      return;
    }

    await remove(
      {
        restaurantId: restaurant.id,
        categoryId: category.id,
        itemId: item.id,
      },
      {
        onError: (error) => {
          addToast(error.detail);
        },
      }
    );
  };

  if (removeStatus.isSuccess) {
    return null;
  }

  return (
    <div className="py-1 mt-2">
      <div>
        <div className="flex items-center justify-between">
          <p className="font-semibold">{item.name}</p>

          {removeStatus.isLoading ? (
            <div>
              <SpinnerIcon className="w-4 h-4 animate-spin text-primary" />
            </div>
          ) : (
            <div className="flex items-center justify-between">
              <button
                type="button"
                className="text-blue-700"
                onClick={() => setIsUpdateFormOpen(true)}
              >
                <PencilIcon className="w-5 h-5" />
              </button>
              <button
                type="button"
                className="text-primary ml-2"
                onClick={onRemove}
                disabled={removeStatus.isLoading}
              >
                <CloseIcon className="w-5 h-5" />
              </button>
            </div>
          )}
        </div>
        {item.description && (
          <p className="text-gray-800 whitespace-pre-line">
            {item.description}
          </p>
        )}
        <p className="mt-1 font-medium text-sm text-red-700">
          £{item.price.toFixed(2)}
        </p>
      </div>

      {isUpdateFormOpen && (
        <UpdateForm
          menu={restaurant.menu}
          category={category}
          item={item}
          close={() => setIsUpdateFormOpen(false)}
        />
      )}
    </div>
  );
};

export default MenuItem;
