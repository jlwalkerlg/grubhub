import { useEffect } from "react";
import { QueryConfig, useQuery } from "react-query";
import useGeocodingServices from "./useGeocodingServices";

export function useGoogleMap(
  divId: string,
  options?: google.maps.MapOptions,
  config: QueryConfig<google.maps.Map> = {}
) {
  const { isReady } = useGeocodingServices();

  const enabled = isReady && (config?.enabled || true);

  const { data: map, refetch } = useQuery(
    divId,
    () => new google.maps.Map(document.getElementById(divId), options),
    {
      staleTime: Infinity,
      refetchOnMount: false,
      refetchOnReconnect: false,
      refetchOnWindowFocus: false,
      ...config,
      enabled,
    }
  );

  useEffect(() => {
    if (enabled) {
      refetch();
    }
  }, [enabled]);

  return { map };
}
