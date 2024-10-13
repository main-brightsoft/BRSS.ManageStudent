using BRSS.ManageStudent.Application.Constants;
using BRSS.ManageStudent.Domain.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace BRSS.ManageStudent.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Console.WriteLine(exception);
            context.Response.ContentType = "application/json";

            var baseException = new BaseException
            {
                TraceId = context.TraceIdentifier,
                MoreInfo = exception.HelpLink
            };

            switch (exception)
            {
                case NotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    baseException.ErrorCode = context.Response.StatusCode;
                    baseException.UserMessage = exception.Message;
                    baseException.DevMessage = exception.Message;
                    break;

                case ConflictException:
                case EmailNotConfirmedException:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    baseException.ErrorCode = exception is EmailNotConfirmedException? CustomStatusCodes.EmailNotConfirmed :context.Response.StatusCode;
                    baseException.UserMessage = exception.Message;
                    baseException.DevMessage = exception.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    baseException.ErrorCode = context.Response.StatusCode;
                    baseException.UserMessage = "An unexpected error occurred.";
#if DEBUG
                    baseException.DevMessage = exception.Message;
#else
                    baseException.DevMessage = "";
#endif
                    break;
            }

            await WriteResponseAsync(context, baseException);
        }

        private async Task WriteResponseAsync(HttpContext context, BaseException baseException)
        {
            await context.Response.WriteAsync(baseException.ToString() ?? "");
        }
    }
}
