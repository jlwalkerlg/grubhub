import * as signalR from "@microsoft/signalr";
import { useEffect } from "react";

interface Options {
  configure?: (connection: signalR.HubConnection) => any;
  onConnect?: () => any;
  onError?: (error: Error) => any;
}

export function useOrdersHub(options?: Options) {
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.NEXT_PUBLIC_API_BASE_URL}/hubs/orders`)
      .build();

    if (options?.configure) {
      options.configure(connection);
    }

    connection.start().then(options?.onConnect).catch(options?.onError);

    return () => connection.stop();
  }, []);
}
