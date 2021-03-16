import { useQueryClient } from "react-query";
import useGeocodingServices from "./useGeocodingServices";

export default function useAddressLookup() {
  const queryClient = useQueryClient();

  const { getGeocoder, refreshSession } = useGeocodingServices();

  const getAddressById = async (id: string) => {
    const cached = queryClient.getQueryData<string>(`address:${id}`);

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
          queryClient.setQueryData<string>(`address:${id}`, address);
          resolve(address);
        });
      });
    });
  };

  return {
    getAddressById,
  };
}
