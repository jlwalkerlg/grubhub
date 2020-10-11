import React from "react";
import { toast } from "react-toastify";
import Swal, { SweetAlertOptions } from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { MenuCategoryDto } from "~/api/restaurants/MenuDto";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import useRestaurants from "~/store/restaurants/useRestaurants";
import MenuItem from "./MenuItem";
import NewMenuItemDropdown from "./NewMenuItemDropdown";
const MySwal = withReactContent(Swal);

interface Props {
  category: MenuCategoryDto;
}

const MenuCategory: React.FC<Props> = ({ category }) => {
  const restaurants = useRestaurants();

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
          <button type="button" className="text-blue-700">
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
