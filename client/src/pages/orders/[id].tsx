import { useRouter } from "next/router";
import React, { FC } from "react";
import useOrder from "~/api/orders/useOrder";

const OrderPage: FC = () => {
  const router = useRouter();
  const orderId = router.query.id?.toString();

  const { data: order } = useOrder(orderId, {
    enabled: !!orderId,
  });

  return (
    <div>
      Order {order.id} status: {order.status}
    </div>
  );
};

export default OrderPage;
