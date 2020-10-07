import React from "react";
import { useForm } from "react-hook-form";
import { MenuCategoryDto, MenuItemDto } from "~/api/restaurants/MenuDto";
import { UpdateMenuItemRequest } from "~/api/restaurants/restaurantsApi";
import { ErrorAlert } from "~/components/Alert/Alert";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import { combineRules, MinRule, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
import useRestaurants from "~/store/restaurants/useRestaurants";
import Swal, { SweetAlertOptions } from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { toast } from "react-toastify";
const MySwal = withReactContent(Swal);

interface Props {
  category: MenuCategoryDto;
  item: MenuItemDto;
}

interface FormValues {
  name: string;
  description: string;
  price: number;
}

const MenuItem: React.FC<Props> = ({ category, item }) => {
  const restaurants = useRestaurants();

  const [isEditFormOpen, setIsEditFormOpen] = React.useState(false);
  const [error, setError] = React.useState(null);

  const [isDeleting, setIsDeleting] = React.useState(false);

  const form = useForm<FormValues>({
    defaultValues: {
      name: item.name,
      description: item.description,
      price: item.price,
    },
  });

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    setError(null);

    const request: UpdateMenuItemRequest = {
      ...data,
      price: +data.price,
    };

    const result = await restaurants.updateMenuItem(
      category.name,
      item.name,
      request
    );

    if (!result.isSuccess) {
      setError(result.error.message);

      if (result.error.isValidationError) {
        setFormErrors(result.error.errors, form);
      }

      return;
    }

    if (data.name === item.name) {
      setIsEditFormOpen(false);
      form.reset(data);
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

  const handleClickDelete = async () => {
    if (isDeleting) return;

    const options: SweetAlertOptions = {
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
    };

    const dialogResult = await MySwal.fire(options);

    if (!dialogResult.isConfirmed) {
      return;
    }

    setIsDeleting(true);

    const result = await restaurants.deleteMenuItem(category.name, item.name);

    if (!result.isSuccess) {
      toast.error(result.error.message);
      setIsDeleting(false);
    }
  };

  return (
    <div className="px-4 py-2">
      <div>
        <div className="flex items-center justify-between">
          <p className="font-semibold">{item.name}</p>
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
        <p className="text-gray-800">{item.description}</p>
        <p className="mt-1 font-medium text-sm text-red-700">£{item.price}</p>
      </div>

      {isEditFormOpen && (
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
              className="btn btn-sm btn-primary"
            >
              Update Item
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
    </div>
  );
};

export default MenuItem;
