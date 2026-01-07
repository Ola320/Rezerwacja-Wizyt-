using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Auth
{
    public class SessionAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CookieName = "session_token";

        public SessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            if (context.Request.Cookies.TryGetValue(CookieName, out var token) &&
                !string.IsNullOrWhiteSpace(token))
            {
                var now = DateTime.UtcNow;

                var session = await db.Sessions
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s =>
                        s.Token == token &&
                        s.RevokedAtUtc == null &&
                        s.ExpiresAtUtc > now);

                if (session != null)
                {
                    context.Items["User"] = session.User;
                    context.Items["Session"] = session;
                }
            }

            await _next(context);
        }
    }
}