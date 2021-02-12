import { CardElement, useElements, useStripe } from "@stripe/react-stripe-js";
import { useMutation } from "react-query";
import { ApiError } from "~/api/Api";
import useAuth from "~/api/users/useAuth";

export default function usePay() {
  const stripe = useStripe();
  const elements = useElements();

  const { user } = useAuth();

  return useMutation<void, ApiError, string, null>(
    async (paymentIntentClientSecret) => {
      if (!stripe || !elements) return;

      try {
        const result = await stripe.confirmCardPayment(
          paymentIntentClientSecret,
          {
            payment_method: {
              card: elements.getElement(CardElement),
              billing_details: {
                name: user.name,
              },
            },
          }
        );

        if (result.error) {
          const error: ApiError = {
            message: result.error.message ?? "Something went wrong.",
            statusCode: 400,
            isValidationError: false,
          };

          throw error;
        }
      } catch (e) {
        const error: ApiError = {
          message: e.message ?? "Something went wrong.",
          statusCode: 400,
          isValidationError: false,
        };

        throw error;
      }
    }
  );
}
