using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BRSS.ManageStudent.Domain.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace BRSS.ManageStudent.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = context.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Issuer"],
                    ValidAudience = context.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(context.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Key"] ?? throw new InvalidOperationException())),
                    ClockSkew = TimeSpan.Zero // Bỏ qua sai số thời gian nếu có
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                context.User = principal; // Gán claims vào context nếu token hợp lệ

                // Kiểm tra quyền truy cập
                var requiredRoles = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>()?.Roles?.Split(','); // Lấy vai trò yêu cầu từ endpoint

                if (requiredRoles != null && requiredRoles.Any())
                {
                    // Kiểm tra xem người dùng có ít nhất một vai trò phù hợp không
                    var userRoles = principal.FindAll(ClaimTypes.Role).Select(r => r.Value);
                    if (!userRoles.Intersect(requiredRoles).Any())
                    {
                        await WriteResponseAsync(context, HttpStatusCode.Forbidden, "Bạn không có quyền truy cập vào tài nguyên này.");
                        return;
                    }
                }
            }
            catch (SecurityTokenExpiredException)
            {
                await WriteResponseAsync(context, HttpStatusCode.Unauthorized, "Token đã hết hạn");
                return;
            }
            catch (Exception)
            {
                await WriteResponseAsync(context, HttpStatusCode.Unauthorized, "Token không hợp lệ");
                return;
            }
        }

        await _next(context);
    }

    private static Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var errorResponse = new BaseException
        {
            ErrorCode = (int)statusCode,
            DevMessage = "Lỗi xác thực JWT",
            UserMessage = message,
            TraceId = context.TraceIdentifier,
            MoreInfo = "Hãy đăng nhập lại để tiếp tục.",
            Errors = new List<string> { message }
        };

        var json = JsonSerializer.Serialize(errorResponse);
        return context.Response.WriteAsync(json);
    }
}
