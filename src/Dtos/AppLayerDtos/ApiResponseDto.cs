namespace Domain.Dtos.AppLayerDtos
{
    public record ApiResponseDto<T>
    {
        public string Message { get; init; }
        public bool Success { get; init; }
        public T Result { get; init; }
    }
}