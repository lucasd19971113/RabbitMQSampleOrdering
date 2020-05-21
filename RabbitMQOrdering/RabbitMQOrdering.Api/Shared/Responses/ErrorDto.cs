namespace RabbitMQOrdering.Api.Shared.Responses
{
     public class ErrorDto
    {
        public Error Error { get; set; }

        public static ErrorDto Create(int status, string detail)
            => new ErrorDto { Error = new Error { Status = status, Detail = detail } };
    }

    public class Error
    {
        public int Status { get; set; }

        /// <summary>
        /// Human redable error
        /// </summary>
        public string Detail { get; set; }
    }
}