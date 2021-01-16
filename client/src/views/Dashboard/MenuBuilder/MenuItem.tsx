import React, { FC } from "react";
import { useForm } from "react-hook-form";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { MenuCategoryDto, MenuDto, MenuItemDto } from "~/api/menu/MenuDto";
import useMenu from "~/api/menu/useMenu";
import useRemoveMenuItem from "~/api/restaurants/useRemoveMenuItem";
import useUpdateMenuItem from "~/api/restaurants/useUpdateMenuItem";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import {
  combineRules,
  MaxLengthRule,
  MinRule,
  RequiredRule,
} from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";

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
      newItemName: item.name,
      description: item.description,
      price: item.price,
    },
  });

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    await update(
      {
        restaurantId: menu.restaurantId,
        categoryName: category.name,
        oldItemName: item.name,
        ...data,
        price: +data.price,
      },
      {
        onSuccess: () => {
          form.setValue("newItemName", data.newItemName);
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
          <ErrorAlert message={error.message} />
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
          name="newItemName"
          id="newItemName"
          data-invalid={!!form.errors.newItemName}
        />
        {form.errors.newItemName && (
          <p className="form-error mt-1">{form.errors.newItemName.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="description">
          Description
        </label>
        <textarea
          ref={form.register({
            validate: combineRules([new MaxLengthRule(280)]),
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
  const { data: menu } = useMenu(user.restaurantId);

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
        restaurantId: menu.restaurantId,
        categoryName: category.name,
        itemName: item.name,
      },
      {
        onError: (error) => {
          addToast(error.message);
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
        <p className="mt-1 font-medium text-sm text-red-700">£{item.price}</p>
      </div>

      {isUpdateFormOpen && (
        <UpdateForm
          menu={menu}
          category={category}
          item={item}
          close={() => setIsUpdateFormOpen(false)}
        />
      )}
    </div>
  );
};

export default MenuItem;
