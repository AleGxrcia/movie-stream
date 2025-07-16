
namespace MovieStream.Core.Application.Wrappers
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public T Data { get; set; }
        public MetaData? MetaData { get; private set; }

        public Response() { }

        public Response(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            MetaData = null;
        }

        public Response(T data, MetaData metaData, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            MetaData = metaData;
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
    }
}
