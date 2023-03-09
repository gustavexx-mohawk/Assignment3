namespace Assignment3.Models
{
    public class Error
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public Guid Id { get; set; }

        public Error(int statusCode, string message)
        {
            Id = Guid.NewGuid();
            Message = message;
            StatusCode = statusCode;
        }

        override
        public string? ToString()
        {
            return "ErrorID: " + Id + "\n" + "StatusCode: " + StatusCode + "\n" + Message;
        }
    }
}
