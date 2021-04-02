using System.Threading.Tasks;

namespace Web.Services.Storage
{
    public interface IImageStore
    {
        Task<ImageUploadParams> GenerateUploadParams(
            string filename,
            string contentType,
            int? maxSize = null,
            int minSize = 0);
    }
}
