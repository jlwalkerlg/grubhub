import { useQuery } from "react-query";
import AddressSearcher, { Coordinates } from "./AddressSearcher";

export function getPostcodeLookupQueryKey(postcode: string) {
  return postcode.trim().replace(" ", "");
}

export default function usePostcodeLookup(postcode: string) {
  return useQuery<Coordinates, Error>(
    getPostcodeLookupQueryKey(postcode),
    () => {
      return new Promise(async (resolve, reject) => {
        try {
          const coords = await AddressSearcher.getCoordinatesByPostcode(
            postcode
          );

          if (coords !== null) {
            return resolve(coords);
          }
        } catch (e) {}

        return reject(new Error("Failed to retrieve coordinates."));
      });
    },
    {
      staleTime: Infinity,
      retry: false,
    }
  );
}
