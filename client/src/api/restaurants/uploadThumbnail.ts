import axios from "axios";

export interface UploadThumbnailRequest {
  url: string;
  inputs: Record<string, string>;
}

export const uploadThumbnail = async (
  thumbnail: File,
  request: UploadThumbnailRequest
) => {
  const data = new FormData();

  for (const field in request.inputs) {
    if (Object.prototype.hasOwnProperty.call(request.inputs, field)) {
      const value = request.inputs[field];
      data.append(field, value);
    }
  }

  data.append("file", thumbnail);

  const response = await axios.post(request.url, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  return response;
};
