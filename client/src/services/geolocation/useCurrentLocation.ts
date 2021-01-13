import { useQuery, useQueryCache } from "react-query";
import Coordinates from "./Coordinates";
import useGeocodingServices from "./useGeocodingServices";
import { getPostcodeLookupQueryKey } from "./usePostcodeLookup";

export default function useCurrentLocation() {
  const { getGeocoder } = useGeocodingServices();

  const cache = useQueryCache();

  const { isFetching, refetch, data } = useQuery<
    { postcode: string; coordinates: Coordinates },
    Error
  >(
    "useCurrentLocation",
    () => {
      return new Promise(async (resolve, reject) => {
        if (!navigator.geolocation) {
          reject(new Error("Geolocation is not supported by this browser."));
          return;
        }

        navigator.geolocation.getCurrentPosition(
          async ({ coords: coordinates }) => {
            const geocoder = await getGeocoder();

            const { latitude, longitude } = coordinates;

            const request: google.maps.GeocoderRequest = {
              location: { lat: latitude, lng: longitude },
            };

            geocoder.geocode(request, (results) => {
              const postcode =
                results[0]?.address_components.filter(
                  (x) => x.types.length === 1 && x.types[0] === "postal_code"
                )[0]?.long_name ?? null;

              if (postcode !== null) {
                cache.setQueryData(
                  getPostcodeLookupQueryKey(postcode),
                  coordinates
                );

                resolve({ postcode, coordinates });
                return;
              }

              reject(new Error("Failed to retrieve postcode."));
            });
          },
          () => reject(new Error("Geolocation services unavailable."))
        );
      });
    },
    {
      staleTime: Infinity,
      retry: false,
      enabled: false,
    }
  );

  const getCurrentLocation = () => {
    return refetch({
      throwOnError: true,
    });
  };

  return {
    ...data,
    isLoading: isFetching,
    getCurrentLocation,
  };
}
