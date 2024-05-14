namespace Middleware
{
    public class JWTInHeaderMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var authenticationCookieName = "token";
            var token = context.Request.Cookies[authenticationCookieName];
            if (token != null)
            {
                context.Request.Headers.Append("Authorization", "Bearer " + token);
            }
            await _next(context);
        }


    }
    public static class JWTInHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTInHeader(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTInHeaderMiddleware>();
        }
    }
}