using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shosh.Data.IRepository;
using Shosh.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Service
{
    public class BanCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BanCleanupService> _logger;

        public BanCleanupService(IServiceScopeFactory serviceScopeFactory, ILogger<BanCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var banService = scope.ServiceProvider.GetRequiredService<IBanService>();
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                        var expiredBans = await banService.GetExpiredBansAsync();
                        foreach (var ban in expiredBans)
                        {
                            if (ban == null || ban.UserId == 0)
                            {
                                Console.WriteLine("❌ HATA: Ban nesnesi geçersiz veya UserId boş.");
                                continue;
                            }

                            // 📌 Kullanıcının gerçekten var olup olmadığını kontrol et
                            var user = await userRepository.GetUserByIdAsync((int)ban.UserId);
                            if (user == null)
                            {
                                Console.WriteLine($"❌ HATA: Kullanıcı bulunamadı (UserID: {ban.UserId})");
                                continue;
                            }

                            // Kullanıcıyı banlıdan çıkart
                            user.IsBanned = false;
                            user.Role = "User"; // 📌 Kullanıcı artık normal kullanıcı oldu
                            await userRepository.UpdateUserAsync(user);

                            await banService.UnbanUserAsync(user.Id);

                            // 🚀 Kullanıcıya bildirim gönder
                            await notificationService.SendNotificationAsync(user.Id, "Ban süreniz sona erdi. Tekrar giriş yapabilirsiniz.");
                            Console.WriteLine($"✅ Kullanıcının banı açıldı: {user.Email}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"🚨 BanCleanupService HATA: {ex.Message}");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Her 1 saatte bir kontrol et
            }
        }
    }
}
