namespace MovieStream.Core.Application.Wrappers
{
    public class Response
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public Error? Error { get; set; }

        public Response(bool succeeded, string? message = null)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public Response(Error error)
        {
            Succeeded = false;
            Error = error;
        }
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
        public MetaData? Meta { get; private set; }

        public Response(T data, string? message = null) : base(true, message)
        {
            Data = data;
            Meta = null;
        }

        public Response(T data, MetaData meta, string? message = null) : base(true, message)
        {
            Data = data;
            Meta = meta;
        }
    }

    public class Error
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
