import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { useIsMounted } from "../../services/useIsMounted";

interface Options {
  configure?: (connection: signalR.HubConnection) => any;
  onConnect?: () => any;
  onError?: (error: Error) => any;
}

export function useOrdersHub(options?: Options) {
  const isMounted = useIsMounted();

  const [isLoading, setIsLoading] = useState(true);
  const [connectionError, setConnectionError] = useState<Error>();

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.NEXT_PUBLIC_API_BASE_URL}/hubs/orders`)
      .build();

    if (options?.configure) {
      options.configure(connection);
    }

    connection
      .start()
      .then(options?.onConnect)
      .catch((error) => {
        if (isMounted) setConnectionError(error);
        options?.onError(error);
      })
      .finally(() => isMounted && setIsLoading(false));

    return () => connection.stop();
  }, []);

  return { isLoading, isConnectionError: !!connectionError, connectionError };
}
