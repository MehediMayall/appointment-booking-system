namespace SharedKernel;

public sealed class IdempotencyMiddleware : IMiddleware
{
    private readonly IHybridCacheService _hybridCache;

    public IdempotencyMiddleware(IHybridCacheService hybridCache)
    {
        _hybridCache = hybridCache;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
    {
        if (context.Request.Method != "POST")
        {
            await _next(context);
            return;
        }

        context.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey);

        if (string.IsNullOrEmpty(idempotencyKey))
        {
            await _next(context);
            return;
        }

        string idempotencyKeyString = idempotencyKey.FirstOrDefault().ToString();

        var cachedResponse = await _hybridCache.GetOrCreateAsync(idempotencyKeyString, async entry => "", TimeSpan.FromMinutes(20));

        if (cachedResponse.IsFailure)
        {
            await _next(context);
            return;
        }

        if (!string.IsNullOrEmpty(cachedResponse.Value))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            try
            {
                await CreateErrorResponse(context, idempotencyKeyString);
            }
            catch (Exception ex) { Log.Error($"IdempotencyMiddleware: {ex.GetAllExceptions()}"); }
            return;
        }

        // Buffer response

        await _next(context); // Process request
        await _hybridCache.SetAsync(idempotencyKeyString, idempotencyKeyString, TimeSpan.FromMinutes(20));

    }
    
    private async Task CreateErrorResponse(HttpContext context, string idempotencyKey)
    {
        Response<string> errorResponse =  Response<string>.Error($"Couldn't process duplicate request. Idempotency key already exist, keys: {idempotencyKey}");

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