import { useQuery, useQueryCache } from "react-query";
import AddressSearcher, { Coordinates } from "./AddressSearcher";
import { getPostcodeLookupQueryKey } from "./usePostcodeLookup";

export default function useCurrentLocation() {
  const cache = useQueryCache();

  const { isFetching, refetch, data } = useQuery<
    { postcode: string; coordinates: Coordinates },
    Error
  >(
    "currentLocation",
    () => {
      if (!navigator.geolocation) {
        throw new Error("Geolocation is not supported by this browser.");
      }

      return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(
          async ({ coords: coordinates }) => {
            const { latitude, longitude } = coordinates;

            try {
              const postcode = await AddressSearcher.getPostcodeByCoordinates(
                latitude,
                longitude
              );

              if (postcode !== null) {
                cache.setQueryData(
                  getPostcodeLookupQueryKey(postcode),
                  coordinates
                );
                return resolve({ postcode, coordinates });
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
    ...data,
    isLoading: isFetching,
    lookup,
  };
}
