namespace Product.Application.Helpers
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string ErrorType { get; set; }
        public string StackTrace { get; set; }
    }
}

