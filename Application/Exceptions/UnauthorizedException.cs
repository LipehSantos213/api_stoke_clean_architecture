namespace api_stoke.Application.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message) 
            : base(message, StatusCodes.Status401Unauthorized) { }
    }
}
