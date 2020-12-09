import { useQuery } from "react-query";
import AddressSearcher from "./AddressSearcher";

export default function usePostcodeLookup() {
  const { isFetching, refetch, data } = useQuery<string, Error>(
    "postcode",
    () => {
      if (!navigator.geolocation) {
        throw new Error("Geolocation is not supported by this browser.");
      }

      return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(
          async ({ coords: { latitude, longitude } }) => {
            try {
              const postcode = await AddressSearcher.getPostcodeByLocation(
                latitude,
                longitude
              );

              if (postcode !== null) {
                return resolve(postcode);
              }
            } catch (e) {}

            return reject("Failed to retrieve postcode.");
          }
        );
      });
    },
    {
      staleTime: Infinity,
      retry: false,
      enabled: false,
    }
  );

  const lookup = async () => {
    return await refetch({
      throwOnError: true,
    });
  };

  return {
    postcode: data,
    isLoading: isFetching,
    lookup,
  };
}
