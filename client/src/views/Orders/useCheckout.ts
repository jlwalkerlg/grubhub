import { CardElement, useElements, useStripe } from "@stripe/react-stripe-js";
import { useState } from "react";
import { useMutation, useQueryCache } from "react-query";
import { ApiError } from "~/api/Api";
import { getActiveOrderQueryKey } from "~/api/orders/useActiveOrder";
import { PlaceOrderCommand, usePlaceOrder } from "~/api/orders/usePlaceOrder";
import { UserDto } from "~/api/users/UserDto";

export default function useCheckout(user: UserDto) {
  const stripe = useStripe();
  const elements = useElements();

  const cache = useQueryCache();

  const [placeOrder] = usePlaceOrder();

  let [secret, setSecret] = useState<string>();

  return useMutation<void, ApiError, PlaceOrderCommand, null>(
    async (command) => {
      if (!stripe || !elements) return;

      if (!secret) {
        secret = await placeOrder(command, {
          throwOnError: true,
        });

        setSecret(secret);
      }

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
        } else if (result.paymentIntent.status === "requires_capture") {
          //
        }
      } catch (e) {
        const error: ApiError = {
          message: e.message ?? "Something went wrong.",
          statusCode: 400,
          isValidationError: false,
        };

        throw error;
      }
    },
    {
      onSuccess: (_, command) => {
        cache.invalidateQueries(getActiveOrderQueryKey(command.restaurantId));
      },
    }
  );
}
