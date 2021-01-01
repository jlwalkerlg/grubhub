import React from "react";
import { useForm } from "react-hook-form";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { MenuCategoryDto } from "~/api/menu/MenuDto";
import useMenu from "~/api/menu/useMenu";
import useRemoveMenuCategory from "~/api/restaurants/useRemoveMenuCategory";
import useRenameMenuCategory from "~/api/restaurants/useRenameMenuCategory";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import { combineRules, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
import MenuItem from "./MenuItem";
import NewMenuItemDropdown from "./NewMenuItemDropdown";

const MySwal = withReactContent(Swal);

interface Props {
  category: MenuCategoryDto;
}

const MenuCategory: React.FC<Props> = ({ category }) => {
  const { addToast } = useToasts();

  const { user } = useAuth();
  const { data: menu } = useMenu(user.restaurantId);

  const [isRenameFormOpen, setIsRenameFormOpen] = React.useState(false);

  const [rename, renameStatus] = useRenameMenuCategory();

  const renameForm = useForm({
    defaultValues: {
      newName: category.name,
    },
  });

  const onRename = renameForm.handleSubmit(async (data) => {
    if (renameForm.formState.isSubmitting) return;

    if (data.newName === category.name) {
      setIsRenameFormOpen(false);
      renameForm.reset(data);
      return;
    }

    await rename(
      {
        restaurantId: menu.restaurantId,
        oldName: category.name,
        ...data,
      },
      {
        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, renameForm);
          }
        },
      }
    );
  });

  const onEdit = () => {
    setIsRenameFormOpen(true);
  };

  const onCancelEdit = () => {
    setIsRenameFormOpen(false);
    renameForm.reset();
    renameStatus.reset();
  };

  const [remove, removeStatus] = useRemoveMenuCategory();

  const onDelete = async () => {
    if (removeStatus.isLoading) return;

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

    await remove(
      {
        restaurantId: menu.restaurantId,
        categoryName: category.name,
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
    <div className="mt-4">
      <div className="rounded bg-gray-100 px-4 py-3 shadow-sm text-primary font-medium flex items-center justify-between">
        <p>{category.name}</p>

        <div className="ml-1">
          {removeStatus.isLoading ? (
            <div>
              <SpinnerIcon className="w-4 h-4 animate-spin" />
            </div>
          ) : (
            <div className="flex items-center justify-between">
              <button
                type="button"
                className="text-blue-700"
                onClick={onEdit}
                disabled={renameForm.formState.isSubmitting}
              >
                <PencilIcon className="w-5 h-5" />
              </button>
              <button
                type="button"
                className="text-primary ml-2"
                onClick={onDelete}
                disabled={removeStatus.isLoading}
              >
                <CloseIcon className="w-5 h-5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {isRenameFormOpen && (
        <form onSubmit={onRename} className="px-2 pb-3">
          {renameStatus.isError && (
            <div className="my-3">
              <ErrorAlert message={renameStatus.error.message} />
            </div>
          )}

          <div className="mt-4">
            <label className="label" htmlFor="newName">
              Name <span className="text-primary">*</span>
            </label>
            <input
              ref={renameForm.register({
                validate: combineRules([new RequiredRule()]),
              })}
              className="input"
              type="text"
              name="newName"
              id="newName"
              data-invalid={!!renameForm.errors.newName}
            />
            {renameForm.errors.newName && (
              <p className="form-error mt-1">
                {renameForm.errors.newName.message}
              </p>
            )}
          </div>

          <div className="mt-4">
            <button
              type="submit"
              disabled={renameForm.formState.isSubmitting}
              className="w-full lg:w-auto btn btn-sm btn-primary mt-3 lg:mt-0"
            >
              Rename
            </button>
            <button
              type="button"
              onClick={onCancelEdit}
              disabled={renameForm.formState.isSubmitting}
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
