import React from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import Swal, { SweetAlertOptions } from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { MenuCategoryDto } from "~/api/restaurants/MenuDto";
import { RenameMenuCategoryRequest } from "~/api/restaurants/restaurantsApi";
import { ErrorAlert } from "~/components/Alert/Alert";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import { combineRules, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
import useRestaurants from "~/store/restaurants/useRestaurants";
import MenuItem from "./MenuItem";
import NewMenuItemDropdown from "./NewMenuItemDropdown";
const MySwal = withReactContent(Swal);

interface Props {
  category: MenuCategoryDto;
}

interface FormValues {
  newName: string;
}

const MenuCategory: React.FC<Props> = ({ category }) => {
  const restaurants = useRestaurants();

  const [isEditFormOpen, setIsEditFormOpen] = React.useState(false);
  const [error, setError] = React.useState(null);

  const form = useForm<FormValues>({
    defaultValues: {
      newName: category.name,
    },
  });

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    if (data.newName === category.name) {
      setIsEditFormOpen(false);
      form.reset(data);
      return;
    }

    setError(null);

    const request: RenameMenuCategoryRequest = {
      newName: data.newName,
    };

    console.log(request);

    const result = await restaurants.renameMenuCategory(category.name, request);

    if (!result.isSuccess) {
      setError(result.error.message);

      if (result.error.isValidationError) {
        setFormErrors(result.error.errors, form);
      }

      return;
    }
  });

  const handleOpenEditForm = () => {
    setIsEditFormOpen(true);
  };

  const handleCancelEdit = () => {
    setIsEditFormOpen(false);
    form.reset();
    setError(null);
  };

  const [isDeleting, setIsDeleting] = React.useState(false);

  const handleClickDelete = async () => {
    if (isDeleting) return;

    const options: SweetAlertOptions = {
      title: <p>Delete category {category.name} from the menu?</p>,
      icon: "warning",
      confirmButtonColor: "#c53030",
      confirmButtonText: "Delete",
      showCancelButton: true,
      cancelButtonColor: "#4a5568",
    };

    const dialogResult = await MySwal.fire(options);

    if (!dialogResult.isConfirmed) {
      return;
    }

    setIsDeleting(true);

    const result = await restaurants.removeMenuCategory(category.name);

    if (!result.isSuccess) {
      toast.error(result.error.message);
      setIsDeleting(false);
    }
  };

  return (
    <div className="mt-4">
      <div className="rounded bg-gray-100 px-4 py-3 shadow-sm text-primary font-medium flex items-center justify-between">
        <p>{category.name}</p>
        <div className="flex items-center justify-between">
          <button
            type="button"
            className="text-blue-700"
            onClick={handleOpenEditForm}
            disabled={form.formState.isSubmitting}
          >
            <PencilIcon className="w-5 h-5" />
          </button>
          <button
            type="button"
            className="text-primary ml-2"
            onClick={handleClickDelete}
            disabled={isDeleting}
          >
            <CloseIcon className="w-5 h-5" />
          </button>
        </div>
      </div>

      {isEditFormOpen && (
        <form onSubmit={onSubmit} className="px-4 pb-3">
          {error && (
            <div className="my-3">
              <ErrorAlert message={error} />
            </div>
          )}

          <div className="mt-4">
            <label className="label" htmlFor="newName">
              Name <span className="text-primary">*</span>
            </label>
            <input
              ref={form.register({
                validate: combineRules([new RequiredRule()]),
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
              disabled={form.formState.isSubmitting}
              className="btn btn-sm btn-primary"
            >
              Rename
            </button>
            <button
              type="button"
              onClick={handleCancelEdit}
              disabled={form.formState.isSubmitting}
              className="btn btn-sm btn-outline-primary ml-2"
            >
              Cancel
            </button>
          </div>
        </form>
      )}

      <div className="p-2">
        {category.items.map((item) => (
          <MenuItem key={item.name} category={category} item={item} />
        ))}

        <NewMenuItemDropdown category={category} />
      </div>
    </div>
  );
};

export default MenuCategory;
