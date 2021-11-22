namespace Supermarket.API.Domain.Services.Communication
{
    public abstract class BaseResponse<T>
    {
        protected BaseResponse(string message)
        {
            Success = false;
            Message = message;
            Resource = default;
        }

        public  bool Success { get; private set; }
        public string Message { get; private set; }
        
        public T Resource { get; private set; }

        public BaseResponse(T resource)
        {
            Success = true;
            Message = string.Empty;
            Resource = resource;
        }
    }
}