import { loadScript } from "../helpers";

export interface AddressSearchResult {
  id: string;
  description: string;
}

export interface Address {
  addressLine1: string;
  addressLine2: string;
  town: string;
  postcode: string;
}

const key = process.env.NEXT_PUBLIC_GOOGLE_API_KEY;

class AddressSearcher {
  private client: google.maps.places.AutocompleteService;
  private geocoder: google.maps.Geocoder;
  private sessionToken: google.maps.places.AutocompleteSessionToken;

  constructor() {
    if (typeof window !== "undefined") {
      if (!window.google) {
        loadScript(
          `https://maps.googleapis.com/maps/api/js?key=${key}&libraries=places`
        ).then(this.init.bind(this));
      } else {
        this.init();
      }
    }
  }

  private init(): void {
    this.client = new window.google.maps.places.AutocompleteService();
    this.geocoder = new window.google.maps.Geocoder();

    this.sessionToken = new google.maps.places.AutocompleteSessionToken();
  }

  search(query: string): Promise<AddressSearchResult[]> {
    const request: google.maps.places.AutocompletionRequest = {
      input: query,
      componentRestrictions: {
        country: "uk",
      },
      types: ["address"],
      sessionToken: this.sessionToken,
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
    const request: google.maps.GeocoderRequest = {
      placeId: id,
    };

    return new Promise((resolve: (address: Address) => void) => {
      this.geocoder.geocode(request, (results) => {
        const { address_components } = results[0];

        const streetNumber = address_components[0].long_name;
        const street = address_components[1].long_name;
        const town = address_components[2].long_name;
        const postalCode = address_components[6].long_name;

        const address: Address = {
          addressLine1: `${streetNumber} ${street}`,
          addressLine2: "",
          town,
          postcode: postalCode,
        };

        resolve(address);
      });
    });
  }
}

export default new AddressSearcher();
