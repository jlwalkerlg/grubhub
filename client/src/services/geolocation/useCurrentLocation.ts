import {
  MutateConfig,
  useMutation,
  useQuery,
  useQueryCache,
} from "react-query";
import Coordinates from "./Coordinates";
import useGeocodingServices from "./useGeocodingServices";
import { getPostcodeLookupQueryKey } from "./usePostcodeLookup";

interface CurrentLocation {
  postcode: string;
  coordinates: Coordinates;
}

export default function useCurrentLocation() {
  const { getGeocoder } = useGeocodingServices();

  const cache = useQueryCache();

  const { data } = useQuery<CurrentLocation, Error>("currentLocation", {
    staleTime: Infinity,
    retry: false,
    enabled: false,
  });

  const [mutate, { isLoading }] = useMutation<
    CurrentLocation,
    Error,
    null,
    null
  >(() => {
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
              cache.setQueryData("currentLocation", coordinates);

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
  });

  const getCurrentLocation = (
    config?: MutateConfig<CurrentLocation, Error, null, null>
  ) => mutate(null, config);

  return {
    location: data,
    isLoading,
    getCurrentLocation,
  };
}
