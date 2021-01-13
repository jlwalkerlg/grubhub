import { debounce } from "lodash";
import { useCallback, useEffect, useState } from "react";
import { useQuery } from "react-query";
import useGeocodingServices from "./useGeocodingServices";

export interface AddressSearchResult {
  id: string;
  description: string;
}

export default function useAddressPredictions(query: string) {
  const [cachedQuery, setCachedQuery] = useState(query);

  const [isPaused, setIsPaused] = useState(false);

  useEffect(() => {
    if (isPaused) {
      setIsPaused(false);
    } else {
      setCachedQuery(query);
    }
    // don't include query in deps!
  }, [query]);

  const pause = useCallback(() => setIsPaused(true), []);

  const { getClient, getSessionToken } = useGeocodingServices();

  const { data, refetch } = useQuery(
    `useAddressPredictions:${cachedQuery}`,
    async () => {
      if (!cachedQuery) return [];

      const client = await getClient();
      const sessionToken = await getSessionToken();

      const request: google.maps.places.AutocompletionRequest = {
        input: cachedQuery,
        componentRestrictions: {
          country: "uk",
        },
        sessionToken: sessionToken,
      };

      return await new Promise(
        (resolve: (predictions: AddressSearchResult[]) => any) => {
          client.getPlacePredictions(
            request,
            (predictions: google.maps.places.AutocompletePrediction[]) => {
              if (predictions === null) {
                resolve([]);
                return;
              }

              resolve(
                predictions.map((x) => ({
                  id: x.place_id,
                  description: x.description,
                }))
              );
            }
          );
        }
      );
    },
    {
      staleTime: Infinity,
      enabled: false,
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      refetchOnReconnect: false,
    }
  );

  const debouncedRefetch = useCallback(debounce(refetch, 1000), []);

  useEffect(() => {
    debouncedRefetch();
  }, [cachedQuery, debouncedRefetch]);

  return { predictions: data ?? [], pause };
}
