using BsbSearch.Services;

namespace BsbSearch.Infrastructure
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next) => _next = next ;
        
        public async Task Invoke(HttpContext context, IPartnerService _partnerService)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("api/") && !context.Request.Path.Value.Contains("api/fakeTeams"))
            {
                if (!context.Request.Headers.Keys.Contains("team-name"))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Team name is missing");
                    return;
                }
                if (!context.Request.Headers.Keys.Contains("very-very-secure"))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Team key is missing");
                    return;
                }
                else
                {
                    if (!await _partnerService.IsKeyValid(context.Request.Headers["team-name"], context.Request.Headers["very-very-secure"]))
                    {
                        context.Response.StatusCode = 401; //UnAuthorized
                        await context.Response.WriteAsync("Invalid User Key");
                        return;
                    }
                }
            }
            await _next.Invoke(context);
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
