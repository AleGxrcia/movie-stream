namespace MovieStream.Core.Application.Wrappers
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public Error Error { get; set; }
        public T Data { get; set; }
        public MetaData? Meta { get; private set; }

        public Response() { }

        public Response(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Meta = null;
        }

        public Response(T data, MetaData meta, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Meta = meta;
        }

        public Response(Error error)
        {
            Succeeded = false;
            Error = error;
        }
    }

    public class Error
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
