using BRSS.ManageStudent.Domain.Exception;
using BRSS.ManageStudent.Domain.Exception.Base;
using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Middleware;

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
        if (exception is NotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(
                text: new BaseException()
                {
                    ErrorCode = ((NotFoundException)exception).ErrorCode,
                    UserMessage = "Không tìm thấy tài nguyên",
                    DevMessage = exception.Message,
                    TraceId = context.TraceIdentifier,
                    MoreInfo = exception.HelpLink,

                }.ToString() ?? "");
        }
        else if (exception is ConflictException)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsync(
                text: new BaseException()
                {
                    ErrorCode = ((ConflictException)exception).ErrorCode,
                    UserMessage = "Lỗi xung đột tài nguyên",
                    DevMessage = exception.Message,
                    TraceId = context.TraceIdentifier,
                    MoreInfo = exception.HelpLink,

                }.ToString() ?? "");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(
                text: new BaseException()
                {
                    ErrorCode = context.Response.StatusCode,
                    UserMessage = "Lỗi hệ thống",
#if DEBUG
                    DevMessage = exception.Message,
#else
                    DevMessage = "",
#endif
                    TraceId = context.TraceIdentifier,
                    MoreInfo = exception.HelpLink,

                }.ToString() ?? "");
        }
    }
}
