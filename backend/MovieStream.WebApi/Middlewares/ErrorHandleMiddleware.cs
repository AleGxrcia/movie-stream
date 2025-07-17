using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Wrappers;
using System.Net;
using System.Text.Json;

namespace MovieStream.WebApi.Middlewares
{
    public class ErrorHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception error)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>();

                switch (error)
                {
                    case ApiException e:
                        response.StatusCode = e.ErrorCode;
                        responseModel.Error = new Error
                        {
                            Code = e.ErrorCode,
                            Message = e.Message,
                            Errors = e.Errors
                        };
                        break;
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Error = new Error
                        {
                            Code = (int)HttpStatusCode.BadRequest,
                            Message = e.Message,
                            Errors = e.Errors
                        };
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Error = new Error
                        {
                            Code = (int)HttpStatusCode.NotFound,
                            Message = e.Message,
                        };
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Error = new Error
                        {
                            Code = (int)HttpStatusCode.InternalServerError,
                            Message = "Internal Server Error. Please try again later."
                        };
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
