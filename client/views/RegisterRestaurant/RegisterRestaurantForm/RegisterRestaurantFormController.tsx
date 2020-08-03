import React, {
  FC,
  FormEvent,
  useState,
  useMemo,
  SyntheticEvent,
  useEffect,
  useRef,
  MouseEvent,
} from "react";
import debounce from "lodash/debounce";
import RegisterRestaurantForm, {
  AutocompleteResult,
} from "./RegisterRestaurantForm";
import { useFormComponent } from "~/lib/Form/useFormComponent";
import {
  RequiredRule,
  PasswordRule,
  EmailRule,
  PhoneRule,
  PostCodeRule,
} from "~/lib/Form/Rule";
import { CompositeForm, Form } from "~/lib/Form/Form";

const getPredictions = (
  client: google.maps.places.AutocompleteService,
  query: string
) => {
  const request: google.maps.places.AutocompletionRequest = {
    input: query,
    componentRestrictions: {
      country: "uk",
    },
    types: ["address"],
  };

  return new Promise((resolve: (predictions: AutocompleteResult[]) => void) => {
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
  });
};

const RegisterRestaurantFormController: FC = () => {
  const managerName = useFormComponent("Jordan Walker", [new RequiredRule()]);
  const managerEmail = useFormComponent("jordan@walker.com", [
    new RequiredRule(),
    new EmailRule(),
  ]);
  const managerPassword = useFormComponent("password123", [
    new RequiredRule(),
    new PasswordRule(),
  ]);
  const restaurantName = useFormComponent("Chow Main", [new RequiredRule()]);
  const restaurantPhone = useFormComponent("01274 788944", [
    new RequiredRule(),
    new PhoneRule(),
  ]);
  const addressLine1 = useFormComponent("", [new RequiredRule()]);
  const addressLine2 = useFormComponent("");
  const city = useFormComponent("", [new RequiredRule()]);
  const postCode = useFormComponent("", [
    new RequiredRule(),
    new PostCodeRule(),
  ]);

  const [manual, setManual] = useState(false);

  const client = useRef<google.maps.places.AutocompleteService>(null);
  useEffect(() => {
    client.current = new window.google.maps.places.AutocompleteService();
  }, []);

  const [autocompleteResults, setAutocompleteResults] = useState<
    AutocompleteResult[]
  >([]);

  useEffect(() => {
    if (addressLine1.value === "") {
      setAutocompleteResults([]);
      return;
    }

    if (manual) {
      setManual(false);
      return;
    }

    const fetchPredictions = debounce(() => {
      getPredictions(client.current, addressLine1.value).then((predictions) => {
        setAutocompleteResults(predictions);
      });
    }, 500);

    fetchPredictions();
  }, [addressLine1.value]);

  function onSelectAddress(e: MouseEvent<HTMLButtonElement>): void {
    e.preventDefault();

    setManual(true);
    setAutocompleteResults([]);

    const placeId = e.currentTarget.dataset.id;

    const geocoder = new google.maps.Geocoder();
    geocoder.geocode(
      {
        placeId,
      },
      (results) => {
        const { address_components } = results[0];

        const streetNumber = address_components[0].long_name;
        const street = address_components[1].long_name;
        const town = address_components[2].long_name;
        const postalCode = address_components[6].long_name;

        addressLine1.setValue(`${streetNumber} ${street}`);
        addressLine2.setValue("");
        city.setValue(town);
        postCode.setValue(postalCode);
      }
    );
  }

  const [step, setStep] = useState(3);

  const form = useMemo(
    () =>
      new CompositeForm([
        new Form({ managerName, managerEmail, managerPassword }),
        new Form({ restaurantName, restaurantPhone }),
        new Form({ addressLine1, addressLine2, city, postCode }),
      ]),
    [
      managerName,
      managerEmail,
      managerPassword,
      restaurantName,
      restaurantPhone,
      addressLine1,
      addressLine2,
      city,
      postCode,
    ]
  );

  const [canAdvance, setCanAdvance] = useState(() => form.validateForm(0));

  useEffect(() => {
    setCanAdvance(form.validateForm(step - 1));
  }, [form]);

  function advanceStep(e: SyntheticEvent) {
    e.preventDefault();

    if (canAdvance) {
      setStep(step + 1);
    }
  }

  function backStep() {
    setStep(step - 1);
  }

  function onSubmit(e: FormEvent) {
    e.preventDefault();

    console.log("values", form.values);
  }

  return (
    <RegisterRestaurantForm
      autocompleteResults={autocompleteResults}
      onSelectAddress={onSelectAddress}
      managerName={managerName}
      managerEmail={managerEmail}
      managerPassword={managerPassword}
      restaurantName={restaurantName}
      restaurantPhone={restaurantPhone}
      addressLine1={addressLine1}
      addressLine2={addressLine2}
      city={city}
      postCode={postCode}
      step={step}
      canAdvance={canAdvance}
      advanceStep={advanceStep}
      backStep={backStep}
      onSubmit={onSubmit}
    />
  );
};

export default RegisterRestaurantFormController;
