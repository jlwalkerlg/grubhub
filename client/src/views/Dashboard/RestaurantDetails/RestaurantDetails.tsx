import { NextPage } from "next";
import Image from "next/image";
import { useRouter } from "next/router";
import React, { FC, useRef, useState } from "react";
import { useQueryClient } from "react-query";
import { confirmBannerUpload } from "~/api/restaurants/confirmBannerUpload";
import { confirmThumbnailUpload } from "~/api/restaurants/confirmThumbnailUpload";
import { generateBannerUploadParams } from "~/api/restaurants/generateBannerUploadParams";
import { generateThumbnailUploadParams } from "~/api/restaurants/generateThumbnailUploadParams";
import { uploadBanner } from "~/api/restaurants/uploadBanner";
import { uploadThumbnail } from "~/api/restaurants/uploadThumbnail";
import useRestaurant, {
  getRestaurantQueryKey,
  RestaurantDto,
} from "~/api/restaurants/useRestaurant";
import useUpdateRestaurantDetails from "~/api/restaurants/useUpdateRestaurantDetails";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert, SuccessAlert } from "~/components/Alert/Alert";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";
import { megabytesToBytes } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";

const RestaurantDetailsForm: FC<{ restaurant: RestaurantDto }> = ({
  restaurant,
}) => {
  const form = useForm({
    defaultValues: {
      name: restaurant.name,
      description: restaurant.description,
      phoneNumber: restaurant.phoneNumber,
      deliveryFee: restaurant.deliveryFee,
      minimumDeliverySpend: restaurant.minimumDeliverySpend,
      maxDeliveryDistanceInKm: restaurant.maxDeliveryDistanceInKm,
      estimatedDeliveryTimeInMinutes: restaurant.estimatedDeliveryTimeInMinutes,
    },
  });

  const rules = useRules({
    name: (b) => b.required(),
    description: (b) => b.required().maxLength(280),
    phoneNumber: (b) => b.required().phone(),
    deliveryFee: (b) => b.required().min(0),
    minimumDeliverySpend: (b) => b.required().min(0),
    maxDeliveryDistanceInKm: (b) => b.required().min(0),
    estimatedDeliveryTimeInMinutes: (b) => b.required().min(1),
  });

  const { mutateAsync: updateRestaurantDetails } = useUpdateRestaurantDetails();

  const onSubmit = form.handleSubmit(async (data) => {
    await updateRestaurantDetails({
      id: restaurant.id,
      ...data,
    });
  });

  return (
    <form onSubmit={onSubmit}>
      {form.error && (
        <div className="my-6">
          <ErrorAlert message={form.error.message} />
        </div>
      )}

      {form.isSuccess && (
        <div className="my-6">
          <SuccessAlert message="Restaurant details updated!" />
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
        <label className="label" htmlFor="phoneNumber">
          Phone Number <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.phoneNumber,
          })}
          className="input"
          type="text"
          name="phoneNumber"
          id="phoneNumber"
          data-invalid={!!form.errors.phoneNumber}
        />
        {form.errors.phoneNumber && (
          <p className="form-error mt-1">{form.errors.phoneNumber.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="deliveryFee">
          Delivery Fee <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.deliveryFee,
          })}
          className="input"
          type="number"
          min="0"
          step="0.01"
          name="deliveryFee"
          id="deliveryFee"
          data-invalid={!!form.errors.deliveryFee}
        />
        {form.errors.deliveryFee && (
          <p className="form-error mt-1">{form.errors.deliveryFee.message}</p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="minimumDeliverySpend">
          Minimum Delivery Spend <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.minimumDeliverySpend,
          })}
          className="input"
          type="number"
          min="0"
          step="0.01"
          name="minimumDeliverySpend"
          id="minimumDeliverySpend"
          data-invalid={!!form.errors.minimumDeliverySpend}
        />
        {form.errors.minimumDeliverySpend && (
          <p className="form-error mt-1">
            {form.errors.minimumDeliverySpend.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="maxDeliveryDistanceInKm">
          Max Delivery Distance (km) <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.maxDeliveryDistanceInKm,
          })}
          className="input"
          type="number"
          min="0"
          step="1"
          name="maxDeliveryDistanceInKm"
          id="maxDeliveryDistanceInKm"
          data-invalid={!!form.errors.maxDeliveryDistanceInKm}
        />
        {form.errors.maxDeliveryDistanceInKm && (
          <p className="form-error mt-1">
            {form.errors.maxDeliveryDistanceInKm.message}
          </p>
        )}
      </div>

      <div className="mt-4">
        <label className="label" htmlFor="estimatedDeliveryTimeInMinutes">
          Estimated Delivery Time (mins) <span className="text-primary">*</span>
        </label>
        <input
          ref={form.register({
            validate: rules.estimatedDeliveryTimeInMinutes,
          })}
          className="input"
          type="number"
          min="5"
          step="5"
          name="estimatedDeliveryTimeInMinutes"
          id="estimatedDeliveryTimeInMinutes"
          data-invalid={!!form.errors.estimatedDeliveryTimeInMinutes}
        />
        {form.errors.estimatedDeliveryTimeInMinutes && (
          <p className="form-error mt-1">
            {form.errors.estimatedDeliveryTimeInMinutes.message}
          </p>
        )}
      </div>

      <div className="mt-5">
        <button
          type="submit"
          disabled={form.isLoading}
          className="btn btn-primary font-semibold w-full"
        >
          Update
        </button>
      </div>
    </form>
  );
};

const RestaurantThumbnailImageForm: FC<{ restaurant: RestaurantDto }> = ({
  restaurant,
}) => {
  const inputRef = useRef<HTMLInputElement>();

  const onClick = () => {
    inputRef.current.click();
  };

  const [imagePreview, setImagePreview] = useState("");

  const imageSrc = imagePreview || restaurant.thumbnail;

  const [file, setFile] = useState<File>();
  const [fileError, setFileError] = useState("");

  const typeWhitelist = ["image/png", "image/jpeg", "image/gif"];
  const maxUploadSizeInMb = 1;

  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    e.preventDefault();

    const file = e.target.files[0];

    if (!typeWhitelist.includes(file.type)) {
      setFileError(
        "File type must be one of: " + typeWhitelist.join(", ") + "."
      );
      return;
    }

    if (file.size === 0) {
      setFileError("File is empty.");
      return;
    }

    if (file.size > megabytesToBytes(maxUploadSizeInMb)) {
      setFileError(
        `File size too big: max upload size is ${maxUploadSizeInMb} MB.`
      );
      return;
    }

    const reader = new FileReader();

    reader.onloadend = () => {
      setFile(file);
      setImagePreview(reader.result.toString());
      setFileError("");
    };

    reader.readAsDataURL(file);
  };

  const form = useForm();

  const queryClient = useQueryClient();

  const onSubmit = form.handleSubmit(async () => {
    setFileError("");

    const { filename, url, inputs } = await generateThumbnailUploadParams({
      filename: file.name,
    });

    await uploadThumbnail(file, { url, inputs });

    await confirmThumbnailUpload({ filename });

    queryClient.invalidateQueries(getRestaurantQueryKey(restaurant.id));

    setImagePreview("");
  });

  return (
    <div className="flex">
      <div className="p-1 border border-gray-300 rounded w-28 h-28">
        <Image
          layout="responsive"
          src={imageSrc}
          width={112}
          height={112}
          className="block cursor-pointer"
          onClick={onClick}
        ></Image>
      </div>

      <form className="ml-6" onSubmit={onSubmit}>
        {fileError && <p className="text-red-700 mb-2">{fileError}</p>}
        {form.error && (
          <p className="text-red-700 mb-2">{form.error.message}</p>
        )}

        <p className="text-gray-600 text-sm">
          Upload a thumbnail image for your restaurant that will be shown in
          search results etc. If not set, a default will be shown.
          <br />
          Max upload size: 1MB.
          <br />
          Recommended dimensions: 112 x 112 px.
        </p>

        <input
          ref={inputRef}
          type="file"
          name="thumbnail"
          id="thumbnail"
          className="hidden"
          onChange={onFileChange}
          accept={typeWhitelist.join(",")}
        />

        <button
          className="btn btn-primary btn-sm normal-case mt-3 inline-flex items-center justify-center w-32 h-9"
          disabled={!imagePreview || form.isLoading}
        >
          {form.isLoading ? (
            <span>
              <SpinnerIcon className="w-4 h-4 animate-spin" />
            </span>
          ) : (
            <span>Upload Image</span>
          )}
        </button>
      </form>
    </div>
  );
};

const RestaurantBannerImageForm: FC<{ restaurant: RestaurantDto }> = ({
  restaurant,
}) => {
  const inputRef = useRef<HTMLInputElement>();

  const onClick = () => {
    inputRef.current.click();
  };

  const [imagePreview, setImagePreview] = useState("");

  const imageSrc = imagePreview || restaurant.banner;

  const [file, setFile] = useState<File>();
  const [fileError, setFileError] = useState("");

  const typeWhitelist = ["image/png", "image/jpeg", "image/gif"];
  const maxUploadSizeInMb = 1;

  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    e.preventDefault();

    const file = e.target.files[0];

    if (!typeWhitelist.includes(file.type)) {
      setFileError(
        "File type must be one of: " + typeWhitelist.join(", ") + "."
      );
      return;
    }

    if (file.size === 0) {
      setFileError("File is empty.");
      return;
    }

    if (file.size > megabytesToBytes(maxUploadSizeInMb)) {
      setFileError(
        `File size too big: max upload size is ${maxUploadSizeInMb} MB.`
      );
      return;
    }

    const reader = new FileReader();

    reader.onloadend = () => {
      setFile(file);
      setImagePreview(reader.result.toString());
      setFileError("");
    };

    reader.readAsDataURL(file);
  };

  const form = useForm();

  const queryClient = useQueryClient();

  const onSubmit = form.handleSubmit(async () => {
    setFileError("");

    const { filename, url, inputs } = await generateBannerUploadParams({
      filename: file.name,
    });

    await uploadBanner(file, { url, inputs });

    await confirmBannerUpload({ filename });

    queryClient.invalidateQueries(getRestaurantQueryKey(restaurant.id));

    setImagePreview("");
  });

  return (
    <div className="flex">
      <div className="p-1 border border-gray-300 rounded w-28 h-28">
        <Image
          layout="responsive"
          src={imageSrc}
          width={112}
          height={112}
          className="block cursor-pointer"
          onClick={onClick}
        ></Image>
      </div>

      <form className="ml-6" onSubmit={onSubmit}>
        {fileError && <p className="text-red-700 mb-2">{fileError}</p>}
        {form.error && (
          <p className="text-red-700 mb-2">{form.error.message}</p>
        )}

        <p className="text-gray-600 text-sm">
          Upload a banner image for your restaurant that will be shown in search
          results etc. If not set, a default will be shown.
          <br />
          Max upload size: 1MB.
          <br />
          Recommended dimensions: 112 x 112 px.
        </p>

        <input
          ref={inputRef}
          type="file"
          name="banner"
          id="banner"
          className="hidden"
          onChange={onFileChange}
          accept={typeWhitelist.join(",")}
        />

        <button
          className="btn btn-primary btn-sm normal-case mt-3 inline-flex items-center justify-center w-32 h-9"
          disabled={!imagePreview || form.isLoading}
        >
          {form.isLoading ? (
            <span>
              <SpinnerIcon className="w-4 h-4 animate-spin" />
            </span>
          ) : (
            <span>Upload Image</span>
          )}
        </button>
      </form>
    </div>
  );
};

const RestaurantDetails: FC = () => {
  const { user } = useAuth();
  const { data: restaurant } = useRestaurant(user.restaurantId);

  return (
    <>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Restaurant Details
      </h2>

      <section>
        <RestaurantDetailsForm restaurant={restaurant} />
      </section>

      <hr className="my-12 border-gray-400" />

      <section className="mt-6">
        <RestaurantThumbnailImageForm restaurant={restaurant} />
      </section>

      <section className="mt-6">
        <RestaurantBannerImageForm restaurant={restaurant} />
      </section>
    </>
  );
};

const RestaurantDetailsWrapper: NextPage = () => {
  const { isLoggedIn, isLoading: isLoadingAuth, user } = useAuth();

  const {
    isLoading: isLoadingRestaurant,
    isError: isErrorLoadingRestaurant,
  } = useRestaurant(user?.restaurantId, {
    enabled: isLoggedIn,
  });

  const isLoading = isLoadingAuth || isLoadingRestaurant;
  const isError = isErrorLoadingRestaurant;

  const router = useRouter();

  if (!isLoading && !isError && user?.role !== "RestaurantManager") {
    router.push("/");
    return null;
  }

  return (
    <DashboardLayout>
      {isLoading && <SpinnerIcon className="w-6 h-6 animate-spin" />}

      {!isLoading && isError && (
        <ErrorAlert message="Restaurant could not be loaded at this time." />
      )}

      {!isLoading && !isError && isLoggedIn && <RestaurantDetails />}
    </DashboardLayout>
  );
};

export default RestaurantDetailsWrapper;
