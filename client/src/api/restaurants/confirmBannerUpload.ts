import api from "../api";

interface ConfirmBannerUploadCommand {
  filename: string;
}

export const confirmBannerUpload = async (
  command: ConfirmBannerUploadCommand
) => {
  await api.put("/restaurant/banner/confirm", command);
};
