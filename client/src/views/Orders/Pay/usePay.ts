import { CardElement, useElements, useStripe } from "@stripe/react-stripe-js";
import { useMutation } from "react-query";
import useAuth from "~/api/users/useAuth";

export default function usePay() {
  const stripe = useStripe();
  const elements = useElements();

  const { user } = useAuth();

  return useMutation<void, Error, string, null>(
    async (paymentIntentClientSecret) => {
      if (!stripe || !elements) return;

      const result = await stripe.confirmCardPayment(
        paymentIntentClientSecret,
        {
          payment_method: {
            card: elements.getElement(CardElement),
            billing_details: {
              name: `${user.firstName} ${user.lastName}`,
            },
          },
        }
      );

      if (result.error) {
        throw result.error;
      }
    }
  );
}
