import { loadScript } from "../utils";

export interface AddressSearchResult {
  id: string;
  description: string;
}

const key = process.env.NEXT_PUBLIC_GOOGLE_API_KEY;

class AddressSearcher {
  private client: google.maps.places.AutocompleteService;
  private geocoder: google.maps.Geocoder;
  private sessionToken: google.maps.places.AutocompleteSessionToken;

  public constructor() {
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

  public search(query: string): Promise<AddressSearchResult[]> {
    const request: google.maps.places.AutocompletionRequest = {
      input: query,
      componentRestrictions: {
        country: "uk",
      },
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

  public getAddress(id: string): Promise<string> {
    const request: google.maps.GeocoderRequest = {
      placeId: id,
    };

    return new Promise((resolve: (address: string) => void) => {
      this.geocoder.geocode(request, (results) => {
        resolve(results[0].formatted_address);
      });
    });
  }
}

export default new AddressSearcher();
