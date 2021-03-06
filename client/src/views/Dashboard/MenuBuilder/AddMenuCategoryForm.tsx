import React from "react";
import useAddMenuCategory from "~/api/menu/useAddMenuCategory";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import PlusIcon from "~/components/Icons/PlusIcon";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";

const AddMenuCategoryForm: React.FC = () => {
  const [isOpen, setIsOpen] = React.useState(false);

  const { user } = useAuth();

  const { mutateAsync: addMenuCategory, reset } = useAddMenuCategory();

  const form = useForm({
    defaultValues: {
      name: "",
    },
  });

  const rules = useRules({
    name: (b) => b.required(),
  });

  const onSubmit = form.handleSubmit(async (data) => {
    await addMenuCategory({
      restaurantId: user.restaurantId,
      ...data,
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
        <span className="ml-2">Add Menu Category</span>
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
            <button
              type="submit"
              disabled={form.isLoading}
              className="w-full lg:w-auto btn btn-sm btn-primary"
            >
              Add Category
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

export default AddMenuCategoryForm;
