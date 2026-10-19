namespace api_stoke.Application.Exceptions
{
    public class UnprocessableException : AppException
    {
        public UnprocessableException(string message) : 
            base(message, StatusCodes.Status422UnprocessableEntity) { }
    }
}
