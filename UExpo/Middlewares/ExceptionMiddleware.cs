using Newtonsoft.Json;
using System.Net;
using UExpo.Api.Model;
using UExpo.Domain.Exceptions;

namespace UExpo.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BaseException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(context, new BaseException("An unexpected error occurred.", (int)HttpStatusCode.InternalServerError));
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, BaseException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.StatusCode;

        ErrorResponse response = new ErrorResponse
        {
            Message = exception.Message,
            StatusCode = exception.StatusCode
        };

        string jsonResponse = JsonConvert.SerializeObject(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
