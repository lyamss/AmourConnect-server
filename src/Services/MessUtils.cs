using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Services
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
}