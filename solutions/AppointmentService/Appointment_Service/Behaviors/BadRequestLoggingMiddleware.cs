namespace AppointmentService;

public sealed class BadRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public BadRequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the response body
        var originalBodyStream = context.Response.Body;
        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;


        await _next(context);


         // Reset the stream position to read the response
        newBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(newBodyStream).ReadToEnd();

        // Log if status code is 400
        if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
        {
            Log.Warning("400 Bad Request detected. Path: {Path}, Response: {Response}",
                context.Request.Path, responseBody);
        }

        // Copy back to original stream
        newBodyStream.Seek(0, SeekOrigin.Begin);
        await newBodyStream.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;
    }

}