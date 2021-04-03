import api from "../apii";

interface GenerateBannerUploadParamsCommand {
  filename: string;
}

export interface GenerateBannerUploadParamsResponse {
  filename: string;
  url: string;
  inputs: Record<string, string>;
}

export const generateBannerUploadParams = async (
  command: GenerateBannerUploadParamsCommand
) => {
  const { data } = await api.post<GenerateBannerUploadParamsResponse>(
    "/restaurant/banner/generate-upload-params",
    command
  );

  return data;
};
