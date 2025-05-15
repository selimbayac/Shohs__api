

using Shosh.Service.IService;

namespace Shosh.API.Middleware
{
    public class BanCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;  // 🔥 Scoped Servis Kullanımı

        public BanCheckMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;  // 🔥 Scoped servisler için Scope Factory
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                // 📌 Kullanıcının "nameid" claim'inin olup olmadığını kontrol et
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim == null)
                {
                    // **HATA ÖNLEME** - Eğer claim yoksa, middleware devam etsin.
                    await _next(context);
                    return;
                }

                if (int.TryParse(userIdClaim.Value, out int userId))
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var banService = scope.ServiceProvider.GetRequiredService<IBanService>();

                        bool isBanned = await banService.IsUserBannedAsync(userId);
                        if (isBanned)
                        {
                            context.Response.StatusCode = 403;
                            await context.Response.WriteAsync("Banlı kullanıcı erişemez.");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
