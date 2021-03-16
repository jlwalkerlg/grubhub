import { useQuery } from "react-query";
import { GOOGLE_AUTOCOMPLETE_CLIENT_KEY } from "~/config";
import useScript from "../useScript";

const src = `https://maps.googleapis.com/maps/api/js?key=${GOOGLE_AUTOCOMPLETE_CLIENT_KEY}&libraries=places`;

export default function useGeocodingServices() {
  const { load: loadScript } = useScript(src);

  const { data, isSuccess, refetch } = useQuery(
    "useGeocodingServices",
    async () => {
      await loadScript();

      return {
        geocoder: new google.maps.Geocoder(),
        client: new google.maps.places.AutocompleteService(),
        sessionToken: new google.maps.places.AutocompleteSessionToken(),
      };
    },
    {
      cacheTime: Infinity,
      staleTime: Infinity,
      refetchOnMount: false,
      refetchOnReconnect: false,
      refetchOnWindowFocus: false,
    }
  );

  const getGeocoder = async () =>
    isSuccess ? data.geocoder : (await refetch()).geocoder;

  const getClient = async () =>
    isSuccess ? data.client : (await refetch()).client;

  const getSessionToken = async () =>
    isSuccess ? data.sessionToken : (await refetch()).sessionToken;

  const refreshSession = () => refetch();

  return {
    isReady: isSuccess,
    getGeocoder,
    getClient,
    getSessionToken,
    refreshSession,
  };
}
