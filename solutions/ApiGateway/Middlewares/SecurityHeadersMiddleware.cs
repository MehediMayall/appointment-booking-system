namespace ApiGateway;

public sealed class SecurityHeadersMiddleware(RequestDelegate request)
{

    public async Task Invoke(HttpContext httpContext)
    {

        // Security Headers
        httpContext.Response.Headers["X-Frame-Options"] = "DENY";
        httpContext.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
        httpContext.Response.Headers["X-Content-Type-Options"] = "nosniff";
        httpContext.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self'; font-src 'self';";
        // httpContext.Response.Headers["Referrer-Policy"] = "same-origin";


        httpContext.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        httpContext.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";


        // Performance Headers
        httpContext.Response.Headers["Connection"] = "Keep-Alive";
        httpContext.Response.Headers["Keep-Alive"] = "timeout=5, max=100";
        httpContext.Response.Headers["X-Powered-By"] = "ASP.NET";
        httpContext.Response.Headers["Server"] = "ASP.NET";
        httpContext.Response.Headers["X-Download-Options"] = "noopen";
        httpContext.Response.Headers["Accept-Encoding"] = "gzip, br";

        await request(httpContext);
    }
}
