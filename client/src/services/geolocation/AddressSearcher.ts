import { loadScript } from "../utils";

const key = process.env.NEXT_PUBLIC_GOOGLE_API_KEY;

export interface AddressSearchResult {
  id: string;
  description: string;
}

export interface Coordinates {
  latitude: number;
  longitude: number;
}

class AddressSearcher {
  private error: boolean = false;

  private client: google.maps.places.AutocompleteService;
  private geocoder: google.maps.Geocoder;
  private sessionToken: google.maps.places.AutocompleteSessionToken;

  public constructor() {
    this.ensureInitialized = this.ensureInitialized.bind(this);

    if (typeof window !== "undefined") {
      if (!window.google) {
        loadScript(
          `https://maps.googleapis.com/maps/api/js?key=${key}&libraries=places`
        )
          .then(this.init.bind(this))
          .catch((error) => {
            this.error = true;
          });
      } else {
        this.init();
      }
    }
  }

  private init() {
    this.client = new window.google.maps.places.AutocompleteService();
    this.geocoder = new window.google.maps.Geocoder();

    this.sessionToken = new google.maps.places.AutocompleteSessionToken();
  }

  private ensureInitialized() {
    return new Promise<void>((resolve, reject) => {
      if (this.client) {
        return resolve();
      }

      if (this.error) {
        return reject(new Error("Failed to load geolocation services."));
      }

      setTimeout(this.ensureInitialized, 200);
    });
  }

  public async search(query: string) {
    await this.ensureInitialized();

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

  public async getAddress(id: string) {
    await this.ensureInitialized();

    const request: google.maps.GeocoderRequest = {
      placeId: id,
    };

    return new Promise((resolve: (address: string) => void) => {
      this.geocoder.geocode(request, (results) => {
        resolve(results[0].formatted_address);
      });
    });
  }

  public async getPostcodeByCoordinates(lat: number, lng: number) {
    await this.ensureInitialized();

    const request: google.maps.GeocoderRequest = {
      location: { lat, lng },
    };

    return new Promise((resolve: (address: string) => void) => {
      this.geocoder.geocode(request, (results) => {
        const postcode =
          results[0]?.address_components.filter(
            (x) => x.types.length === 1 && x.types[0] === "postal_code"
          )[0]?.long_name ?? null;

        resolve(postcode);
      });
    });
  }

  public async getCoordinatesByPostcode(postcode: string) {
    await this.ensureInitialized();

    const request: google.maps.GeocoderRequest = {
      address: postcode,
    };

    return new Promise((resolve: (address: Coordinates) => void, reject) => {
      this.geocoder.geocode(request, (results) => {
        const location = results[0]?.geometry.location;

        if (location) {
          resolve({
            latitude: location.lat(),
            longitude: location.lng(),
          });
        } else {
          reject(new Error("Failed to retrieve postcode location."));
        }
      });
    });
  }
}

export default new AddressSearcher();
