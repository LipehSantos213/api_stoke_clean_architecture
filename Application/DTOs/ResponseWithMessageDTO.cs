namespace api_stoke.Application.DTOs
{
    public class ResponseWithMessageDTO
    {
        public string Message { get; set; }

        public static ResponseWithMessageDTO setMessage(string message)
        {
            return new ResponseWithMessageDTO
            {
                Message = message
            };
        }
    }
}
