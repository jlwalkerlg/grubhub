import { useRouter } from "next/router";
import React, { FC } from "react";
import useOrder from "~/api/orders/useOrder";

const OrderPage: FC = () => {
  const router = useRouter();
  const orderId = router.query.id?.toString();
  const isRouterLoading = !orderId;

  const { data: order, isLoading, isError, error } = useOrder(orderId, {
    enabled: !isRouterLoading,
  });

  if (isLoading || isRouterLoading) {
    return <p>Loading</p>;
  }

  if (isError) {
    // TODO: why does this evaluate to true after checkout?
    console.log(error);
    return <p>Error: {error?.message}</p>;
  }

  return (
    <div>
      Order {order?.id} status: {order?.status}
    </div>
  );
};

export default OrderPage;
