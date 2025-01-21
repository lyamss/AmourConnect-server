using Domain.Dtos.AppLayerDtos;

namespace Application.Services
{
    public class ExceptionAPI(bool success, string message, object result) : Exception(message)
    {
        public bool Success { get; } = success;
        public object Result { get; } = result;

        public string Message {  get; } = message;

        public ApiResponseDto<T> ManageApiMessage<T>()
        {
            return new ApiResponseDto<T>
            {
                Message = Message,
                Success = Success,
                Result = (T)Result
            };
        }
    }
}