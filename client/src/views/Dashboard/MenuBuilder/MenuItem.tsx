import React from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { MenuCategoryDto, MenuItemDto } from "~/api/menu/MenuDto";
import useMenu from "~/api/menu/useMenu";
import useRemoveMenuItem from "~/api/restaurants/useRemoveMenuItem";
import useUpdateMenuItem from "~/api/restaurants/useUpdateMenuItem";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { combineRules, MinRule, RequiredRule } from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
const MySwal = withReactContent(Swal);

interface Props {
  category: MenuCategoryDto;
  item: MenuItemDto;
}

const MenuItem: React.FC<Props> = ({ category, item }) => {
  const { user } = useAuth();
  const { data: menu } = useMenu(user.restaurantId);

  const [isUpdateFormOpen, setIsUpdateFormOpen] = React.useState(false);

  const [update, updateStatus] = useUpdateMenuItem();

  const updateForm = useForm({
    defaultValues: {
      newItemName: item.name,
      description: item.description,
      price: item.price,
    },
  });

  const onSubmit = updateForm.handleSubmit(async (data) => {
    if (updateForm.formState.isSubmitting) return;

    await update(
      {
        restaurantId: menu.restaurantId,
        category: category.name,
        item: item.name,
        request: {
          ...data,
          price: +data.price,
        },
      },
      {
        onSuccess: () => {
          setIsUpdateFormOpen(false);
        },
        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, updateForm);
          }
        },
      }
    );
  });

  const onEdit = () => {
    setIsUpdateFormOpen(true);
  };

  const onCancelEdit = () => {
    setIsUpdateFormOpen(false);
    updateForm.reset();
    updateStatus.reset();
  };

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
        category: category.name,
        item: item.name,
      },
      {
        onError: (error) => {
          toast.error(error.message);
        },
      }
    );
  };

  if (removeStatus.isSuccess) {
    return null;
  }

  return (
    <div className="px-4 py-2">
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
                onClick={onEdit}
                disabled={updateForm.formState.isSubmitting}
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
        <p className="text-gray-800">{item.description}</p>
        <p className="mt-1 font-medium text-sm text-red-700">£{item.price}</p>
      </div>

      {isUpdateFormOpen && (
        <form onSubmit={onSubmit} className="px-4 pb-3">
          {updateStatus.isError && (
            <div className="my-3">
              <ErrorAlert message={updateStatus.error.message} />
            </div>
          )}

          <div className="mt-4">
            <label className="label" htmlFor="name">
              Name <span className="text-primary">*</span>
            </label>
            <input
              ref={updateForm.register({
                validate: combineRules([new RequiredRule()]),
              })}
              className="input"
              type="text"
              name="newItemName"
              id="newItemName"
              data-invalid={!!updateForm.errors.newItemName}
            />
            {updateForm.errors.newItemName && (
              <p className="form-error mt-1">
                {updateForm.errors.newItemName.message}
              </p>
            )}
          </div>

          <div className="mt-4">
            <label className="label" htmlFor="description">
              Description <span className="text-primary">*</span>
            </label>
            <textarea
              ref={updateForm.register({
                validate: combineRules([new RequiredRule()]),
              })}
              className="input"
              name="description"
              id="description"
              data-invalid={!!updateForm.errors.description}
            ></textarea>
            {updateForm.errors.description && (
              <p className="form-error mt-1">
                {updateForm.errors.description.message}
              </p>
            )}
          </div>

          <div className="mt-4">
            <label className="label" htmlFor="price">
              Price <span className="text-primary">*</span>
            </label>
            <input
              ref={updateForm.register({
                validate: combineRules([new RequiredRule(), new MinRule(0)]),
              })}
              className="input"
              type="number"
              min="0"
              step="0.01"
              name="price"
              id="price"
              data-invalid={!!updateForm.errors.price}
            />
            {updateForm.errors.price && (
              <p className="form-error mt-1">
                {updateForm.errors.price.message}
              </p>
            )}
          </div>

          <div className="mt-4">
            <button
              type="submit"
              disabled={updateForm.formState.isSubmitting}
              className="btn btn-sm btn-primary"
            >
              Update Item
            </button>
            <button
              type="button"
              onClick={onCancelEdit}
              disabled={updateForm.formState.isSubmitting}
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
