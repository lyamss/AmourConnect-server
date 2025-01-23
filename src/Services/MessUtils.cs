namespace API.Services
{
    public class MessUtils : IMessUtils
    {
        public async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile image)
        {
            if (image == null)
                return null;
            
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public interface IMessUtils
    {
        Task<byte[]> ConvertImageToByteArrayAsync(IFormFile image);
    }
}