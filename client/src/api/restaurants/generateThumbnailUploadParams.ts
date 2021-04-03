import api from "../apii";

interface GenerateThumbnailUploadParamsCommand {
  filename: string;
}

export interface GenerateThumbnailUploadParamsResponse {
  filename: string;
  url: string;
  inputs: Record<string, string>;
}

export const generateThumbnailUploadParams = async (
  command: GenerateThumbnailUploadParamsCommand
) => {
  const { data } = await api.post<GenerateThumbnailUploadParamsResponse>(
    "/restaurant/thumbnail/generate-upload-params",
    command
  );

  return data;
};
