export interface AddressSearchResult {
  id: string;
  description: string;
}

interface Address {
  addressLine1: string;
  addressLine2: string;
  city: string;
  postCode: string;
}

class AddressSearcher {
  private client: google.maps.places.AutocompleteService;
  private geocoder: google.maps.Geocoder;

  constructor() {
    if (typeof window !== "undefined") {
      this.client = new window.google.maps.places.AutocompleteService();
      this.geocoder = new window.google.maps.Geocoder();
    }
  }

  search(query: string): Promise<AddressSearchResult[]> {
    const request: google.maps.places.AutocompletionRequest = {
      input: query,
      componentRestrictions: {
        country: "uk",
      },
      types: ["address"],
    };

    return new Promise(
      (resolve: (predictions: AddressSearchResult[]) => void) => {
        this.client.getPlacePredictions(
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
  }

  getAddress(id: string): Promise<Address> {
    return new Promise((resolve: (address: Address) => void) => {
      this.geocoder.geocode(
        {
          placeId: id,
        },
        (results) => {
          const { address_components } = results[0];

          const streetNumber = address_components[0].long_name;
          const street = address_components[1].long_name;
          const town = address_components[2].long_name;
          const postalCode = address_components[6].long_name;

          const address: Address = {
            addressLine1: `${streetNumber} ${street}`,
            addressLine2: "",
            city: town,
            postCode: postalCode,
          };

          resolve(address);
        }
      );
    });
  }
}

export default new AddressSearcher();
