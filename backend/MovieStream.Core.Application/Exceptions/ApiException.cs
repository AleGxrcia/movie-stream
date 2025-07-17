using System.Globalization;

namespace MovieStream.Core.Application.Exceptions
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }
        public List<string>? Errors { get; set; }

        public ApiException() : base() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ApiException(string message, int errorCode, List<string> errors) : base(message)
        {
            ErrorCode = errorCode;
            Errors = errors;
        }

        public ApiException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }

    }
}
