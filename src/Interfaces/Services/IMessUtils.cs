using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    internal interface IMessUtils
    {
        Task<byte[]> ConvertImageToByteArrayAsync(IFormFile image);
    }
}