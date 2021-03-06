import React, { FC, useState } from "react";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import useRemoveMenuCategory from "~/api/menu/useRemoveMenuCategory";
import useRenameMenuCategory from "~/api/menu/useRenameMenuCategory";
import useRestaurant, {
  MenuCategoryDto,
} from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";
import MenuItem from "./MenuItem";
import NewMenuItemDropdown from "./NewMenuItemDropdown";

const MySwal = withReactContent(Swal);

const MenuCategory: FC<{
  category: MenuCategoryDto;
}> = ({ category }) => {
  const { addToast } = useToasts();

  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);

  const [isFormOpen, setIsFormOpen] = useState(false);

  const {
    mutateAsync: rename,
    reset: resetRenameMutation,
  } = useRenameMenuCategory();

  const form = useForm({
    defaultValues: {
      newName: category.name,
    },
  });

  const rules = useRules({
    newName: (b) => b.required(),
  });

  const onRename = form.handleSubmit(async (data) => {
    if (data.newName === category.name) {
      setIsFormOpen(false);
      form.reset(data);
      return;
    }

    await rename({
      restaurantId: restaurant.id,
      categoryId: category.id,
      ...data,
    });
  });

  const onEdit = () => {
    setIsFormOpen(true);
  };

  const onCancelEdit = () => {
    setIsFormOpen(false);
    form.reset();
    resetRenameMutation();
  };

  const {
    mutate: remove,
    isLoading: isRemoving,
    isSuccess: isRemoveSuccess,
  } = useRemoveMenuCategory();

  const onDelete = async () => {
    if (isRemoving) return;

    const confirmation = await MySwal.fire({
      title: <p>Delete category {category.name} from the menu?</p>,
      icon: "warning",
      confirmButtonColor: "#c53030",
      confirmButtonText: "Delete",
      showCancelButton: true,
      cancelButtonColor: "#4a5568",
    });

    if (!confirmation.isConfirmed) {
      return;
    }

    remove(
      {
        restaurantId: restaurant.id,
        categoryId: category.id,
      },
      {
        onError: (error) => {
          addToast(error.message);
        },
      }
    );
  };

  if (isRemoveSuccess) {
    return null;
  }

  return (
    <div className="mt-4">
      <div className="rounded bg-gray-100 px-4 py-3 shadow-sm text-primary font-medium flex items-center justify-between">
        <p>{category.name}</p>

        <div className="ml-1">
          {isRemoving ? (
            <div>
              <SpinnerIcon className="w-4 h-4 animate-spin" />
            </div>
          ) : (
            <div className="flex items-center justify-between">
              <button
                type="button"
                className="text-blue-700"
                onClick={onEdit}
                disabled={form.isLoading}
              >
                <PencilIcon className="w-5 h-5" />
              </button>
              <button
                type="button"
                className="text-primary ml-2"
                onClick={onDelete}
                disabled={isRemoving}
              >
                <CloseIcon className="w-5 h-5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {isFormOpen && (
        <form onSubmit={onRename} className="px-2 pb-3">
          {form.error && (
            <div className="my-3">
              <ErrorAlert message={form.error.message} />
            </div>
          )}

          <div className="mt-4">
            <label className="label" htmlFor="newName">
              Name <span className="text-primary">*</span>
            </label>
            <input
              ref={form.register({
                validate: rules.newName,
              })}
              className="input"
              type="text"
              name="newName"
              id="newName"
              data-invalid={!!form.errors.newName}
            />
            {form.errors.newName && (
              <p className="form-error mt-1">{form.errors.newName.message}</p>
            )}
          </div>

          <div className="mt-4">
            <button
              type="submit"
              disabled={form.isLoading}
              className="w-full lg:w-auto btn btn-sm btn-primary mt-3 lg:mt-0"
            >
              Rename
            </button>
            <button
              type="button"
              onClick={onCancelEdit}
              disabled={form.isLoading}
              className="w-full lg:w-auto btn btn-sm btn-outline-primary mt-3 lg:mt-0 lg:ml-2"
            >
              Cancel
            </button>
          </div>
        </form>
      )}

      <div className="px-2 mt-2">
        {category.items.map((item) => (
          <MenuItem key={item.name} category={category} item={item} />
        ))}

        <NewMenuItemDropdown category={category} />
      </div>
    </div>
  );
};

export default MenuCategory;
