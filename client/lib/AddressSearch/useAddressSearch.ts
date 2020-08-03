import { useState, useRef } from "react";
import debounce from "lodash/debounce";

import addressSearcher, {
  AddressSearchResult,
  Address,
} from "./AddressSearcher";

export default function useAddressSearch() {
  const [results, setResults] = useState<AddressSearchResult[]>([]);
  const [address, setAddress] = useState<Address>(null);

  const search = useRef(
    debounce((query: string) => {
      addressSearcher.search(query).then(setResults);
    }, 500)
  );

  const getAddress = (id: string) => {
    addressSearcher.getAddress(id).then(setAddress);
  };

  const reset = () => {
    setResults([]);
  };

  return {
    results,
    address,
    search: search.current,
    getAddress,
    reset,
  };
}
