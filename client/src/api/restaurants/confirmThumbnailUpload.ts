import api from "../api";

interface ConfirmThumbnailUploadCommand {
  filename: string;
}

export const confirmThumbnailUpload = async (
  command: ConfirmThumbnailUploadCommand
) => {
  await api.put("/restaurant/thumbnail/confirm", command);
};
