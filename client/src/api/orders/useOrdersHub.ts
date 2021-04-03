import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { API_BASE_URL } from "~/config";
import { useIsMounted } from "../../services/useIsMounted";

interface Options {
  enabled?: boolean;
  configure?: (connection: signalR.HubConnection) => any;
  onConnect?: () => any;
  onError?: (error: Error) => any;
}

export function useOrdersHub(options?: Options) {
  const isMounted = useIsMounted();

  const [isLoading, setIsLoading] = useState(true);
  const [connectionError, setConnectionError] = useState<Error>();

  const enabled = options?.enabled ?? true;

  useEffect(() => {
    if (!enabled) return;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${API_BASE_URL}/hubs/orders`)
      .build();

    if (options?.configure) {
      options.configure(connection);
    }

    connection
      .start()
      .then(options?.onConnect)
      .catch((error) => {
        if (isMounted) setConnectionError(error);
        if (options?.onError) options.onError(error);
      })
      .finally(() => isMounted && setIsLoading(false));

    return () => {
      if (enabled) {
        connection.stop();
      }
    };
  }, [enabled]);

  return { isLoading, isConnectionError: !!connectionError, connectionError };
}
