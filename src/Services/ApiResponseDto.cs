namespace API.Services
{
    public sealed record ApiResponseDto
    {
        private ApiResponseDto(bool suc, object msg, object? objt)
        {
            this.SuccesResponse = suc;
            this.Message = msg;
            this.Result = objt;
        }
        public object Message { get; }
        public bool SuccesResponse { get; }
        public object? Result { get; }
        public static ApiResponseDto Success(object msg, object obj) => new(true, msg, obj);
        public static ApiResponseDto Failure(object msg) => new(false, msg, null);
    }
}