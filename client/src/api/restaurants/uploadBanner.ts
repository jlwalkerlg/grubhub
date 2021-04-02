import axios from "axios";

export interface UploadBannerRequest {
  url: string;
  inputs: Record<string, string>;
}

export const uploadBanner = async (
  banner: File,
  request: UploadBannerRequest
) => {
  const data = new FormData();

  for (const field in request.inputs) {
    if (Object.prototype.hasOwnProperty.call(request.inputs, field)) {
      const value = request.inputs[field];
      data.append(field, value);
    }
  }

  data.append("file", banner);

  const response = await axios.post(request.url, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  return response;
};
