import { CardElement, useElements, useStripe } from "@stripe/react-stripe-js";
import { useMutation } from "react-query";
import Api, { ApiError } from "~/api/Api";
import { PlaceOrderCommand, usePlaceOrder } from "~/api/orders/usePlaceOrder";
import useAuth from "~/api/users/useAuth";

export default function useCheckout() {
  const stripe = useStripe();
  const elements = useElements();

  const { user } = useAuth();

  const [placeOrder] = usePlaceOrder();

  return useMutation<void, ApiError, PlaceOrderCommand, null>(
    async (command) => {
      if (!stripe || !elements) return;

      const secret = await placeOrder(command, {
        throwOnError: true,
      });

      try {
        const result = await stripe.confirmCardPayment(secret, {
          payment_method: {
            card: elements.getElement(CardElement),
            billing_details: {
              name: user.name,
            },
          },
        });

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

      await Api.post(`/orders/${command.orderId}/confirm`);
    }
  );
}
