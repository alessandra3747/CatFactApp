using System.Net;
using System.Text.Json;
using CatFactApplication.Exceptions;

namespace CatFactApplication.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (FailedDeserializationException e)
        {
            Console.WriteLine($"Failed deserialization exception caught: {e.Message}");
            Console.WriteLine(e.StackTrace);

            await WriteErrorResponseAsync(context, HttpStatusCode.InternalServerError, "A data processing error occurred.");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Argument exception caught: {e.Message}");
            
            await WriteErrorResponseAsync(context, HttpStatusCode.BadRequest, e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine($"Unauthorized access exception caught: {e.Message}");
            
            await WriteErrorResponseAsync(context, HttpStatusCode.Unauthorized, "Authentication required or access denied.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unhandled exception caught: {e.Message}");
            Console.WriteLine(e.StackTrace);
            
            await WriteErrorResponseAsync(context, HttpStatusCode.InternalServerError, "An unexpected server error occurred.");
        }
    }
    
    
    private async Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, string errorMessage)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = errorMessage }));
    }
    
}