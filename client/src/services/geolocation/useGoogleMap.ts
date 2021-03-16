import { useEffect, useRef } from "react";
import { useQuery, UseQueryOptions } from "react-query";
import useGeocodingServices from "./useGeocodingServices";

export function useGoogleMap(
  divId: string,
  options?: google.maps.MapOptions,
  config: UseQueryOptions<google.maps.Map> = {}
) {
  const { isReady } = useGeocodingServices();

  const { data: container } = useQuery(
    "useGoogleMap_container_" + divId,
    () => {
      const container = document.createElement("div");
      container.id = "useGoogleMap_container_" + divId;
      container.style.height = "100%";
      container.style.width = "100%";
      container.style.position = "absolute";
      container.style.top = "0px";
      container.style.left = "0px";
      return container;
    },
    {
      staleTime: Infinity,
      enabled: isReady && (config?.enabled || true),
    }
  );

  const { data: map, isSuccess } = useQuery(
    divId,
    () => new google.maps.Map(container, options),
    {
      staleTime: Infinity,
      ...config,
      enabled: container !== undefined,
    }
  );

  useEffect(() => {
    if (container) {
      document.getElementById(divId).appendChild(container);
    }
  }, [divId, container]);

  const alreadyInitialisedRef = useRef(isSuccess);

  return { map, alreadyInitialised: alreadyInitialisedRef.current };
}
