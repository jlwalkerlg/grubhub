import { useState, useRef, useEffect } from "react";
import debounce from "lodash/debounce";

import addressSearcher, { AddressSearchResult } from "./AddressSearcher";

export default function useAddressSearch(query: string) {
  const [isOpen, setIsOpen] = useState(false);
  const [results, setResults] = useState<AddressSearchResult[]>([]);
  const [address, setAddress] = useState<string>(null);

  const search = useRef(
    debounce((query: string) => {
      addressSearcher.search(query).then(setResults);
    }, 500)
  ).current;

  const onSelectAddress = useRef((id: string) => {
    addressSearcher.getAddress(id).then((address) => {
      setIsOpen(false);
      setResults([]);
      setAddress(address);
    });
  }).current;

  useEffect(() => {
    if (query === "") {
      setIsOpen(false);
      setResults([]);
    } else {
      if (isOpen) {
        search(query);
      } else {
        setIsOpen(true);
      }
    }
  }, [query]);

  return {
    results,
    address,
    onSelectAddress,
  };
}
