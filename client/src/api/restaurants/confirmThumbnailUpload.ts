import api from "../apii";

interface ConfirmThumbnailUploadCommand {
  filename: string;
}

export const confirmThumbnailUpload = async (
  command: ConfirmThumbnailUploadCommand
) => {
  await api.put("/restaurant/thumbnail/confirm", command);
};
