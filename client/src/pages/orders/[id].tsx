import * as signalR from "@microsoft/signalr";
import { useRouter } from "next/router";
import React, { FC, useEffect } from "react";
import { useQueryCache } from "react-query";
import useOrder, { getOrderQueryKey } from "~/api/orders/useOrder";
import { AuthLayout } from "~/components/Layout/Layout";

const Order: FC = () => {
  const router = useRouter();
  const orderId = router.query.id?.toString();

  const { data: order } = useOrder(orderId, { enabled: !!orderId });

  const cache = useQueryCache();

  useEffect(() => {
    if (!orderId) return;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5000/hubs/orders")
      .build();

    connection.on(`order_${orderId}.updated`, () => {
      cache.invalidateQueries(getOrderQueryKey(orderId));
    });

    connection
      .start()
      .then(() => cache.invalidateQueries(getOrderQueryKey(orderId)))
      .catch(console.log);

    return () => orderId && connection.stop();
  }, [orderId]);

  return (
    <div>
      {order && (
        <p>
          Order ({order.id}) status: {order.status}
        </p>
      )}
    </div>
  );
};

const OrderPage: FC = () => {
  return (
    <AuthLayout title="Your Order">
      <Order />
    </AuthLayout>
  );
};

export default OrderPage;
