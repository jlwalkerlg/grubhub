import { useQuery } from "react-query";
import api, { ApiError } from "../apii";

export interface CuisineDto {
  name: string;
}

export function getCuisinesQueryKey() {
  return "cuisines";
}

async function getCuisines() {
  const response = await api.get("/cuisines");
  return response.data;
}

export default function useCuisines() {
  return useQuery<CuisineDto[], ApiError>(getCuisinesQueryKey(), getCuisines);
}
