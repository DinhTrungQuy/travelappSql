using TravelAppAPI.Services;

namespace TravelAppAPI.Middleware
{
    public class CheckJwtToken(RequestDelegate next, CacheServices cacheServices)
    {
        private readonly RequestDelegate _next = next;
        private readonly CacheServices _cacheServices = cacheServices;

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var TokenInRedis = _cacheServices.GetData<bool>(token);
                if (!TokenInRedis)
                {
                    context.Request.Headers.Authorization = "";
                    //context.Request.Headers.Authorization.FirstOrDefault()?.Replace(token, "");
                    //var strToken = context.Request.Headers.Authorization.FirstOrDefault();
                }
            }
            await _next(context);
        }

    }
    public static class CheckJwtTokenExtensions
    {
        public static IApplicationBuilder UseCheckJwtToken(
                           this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckJwtToken>();
        }
    }
}
