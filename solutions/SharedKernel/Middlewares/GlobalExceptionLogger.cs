namespace SharedKernel;

public sealed class GlobalExceptionsLogger : IMiddleware
{
   

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex){await CreateErrorResponse(ex, context);}
    }

    private async Task CreateErrorResponse(Exception ex, HttpContext context)
    {
        Response<string> errorResponse =  Response<string>.Error(ex);

        // SAVE LOG
        try { Log.Error(errorResponse.Errors.FirstOrDefault().Message); } catch { }

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "Application/json";

         // Configure JsonSerializerOptions for camelCase
        var jsonOptions = new JsonSerializerOptions 
        { 
            WriteIndented = false, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // Use camelCase naming policy
        };

        // Serialize the error response with camelCase settings
        var json = JsonSerializer.Serialize(errorResponse, jsonOptions);

        await context.Response.WriteAsync(json);
    }   
}