import { useQueryCache } from "react-query";
import useGeocodingServices from "./useGeocodingServices";

export default function useAddressLookup() {
  const cache = useQueryCache();

  const { getGeocoder, refreshSession } = useGeocodingServices();

  const getAddressById = async (id: string) => {
    const cached = cache.getQueryData<string>(`address:${id}`);

    if (cached) {
      return cached;
    }

    const geocoder = await getGeocoder();

    const request: google.maps.GeocoderRequest = {
      placeId: id,
    };

    return new Promise((resolve: (address: string) => any) => {
      geocoder.geocode(request, (results) => {
        // TODO: tidy up?
        refreshSession().then(() => {
          const address = results[0].formatted_address;
          cache.setQueryData<string>(`address:${id}`, address);
          resolve(address);
        });
      });
    });
  };

  return {
    getAddressById,
  };
}
