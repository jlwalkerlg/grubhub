import { useQuery } from "react-query";
import Coordinates from "./Coordinates";
import useGeocodingServices from "./useGeocodingServices";

export function getPostcodeLookupQueryKey(postcode: string) {
  return postcode.trim().replace(" ", "");
}

export default function usePostcodeLookup(postcode: string) {
  const { getGeocoder } = useGeocodingServices();

  return useQuery<Coordinates, Error>(
    getPostcodeLookupQueryKey(postcode),
    () => {
      return new Promise(
        async (resolve: (address: Coordinates) => void, reject) => {
          const geocoder = await getGeocoder();

          const request: google.maps.GeocoderRequest = {
            address: postcode,
          };

          geocoder.geocode(request, (results) => {
            const location = results[0]?.geometry.location;

            if (!location) {
              reject(new Error("Failed to retrieve postcode."));
              return;
            }

            const coords = {
              latitude: location.lat(),
              longitude: location.lng(),
            };

            resolve(coords);
          });
        }
      );
    },
    {
      staleTime: Infinity,
      retry: false,
    }
  );
}
