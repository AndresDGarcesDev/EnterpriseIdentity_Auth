using EnterpriseIdentity_Auth.Shared;
using System.Text.Json;
using Volo.Abp;

namespace EnterpriseIdentity_Auth.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            int statusCode;
            string message;
            string code;

            switch (ex)
            {
                case BusinessException be:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = be.Message;
                    code = be.Code;
                    break;

                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = "Not authorized.";
                    code = "UNAUTHORIZED";
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = "Resource not found.";
                    code = "NOT_FOUND";
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Error internal server.";
                    code = "INTERNAL_SERVER_ERROR";
                    break;
            }

            response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
                Code = code,
                Data = null,
            });

            return response.WriteAsync(result);
        }
    }
}
