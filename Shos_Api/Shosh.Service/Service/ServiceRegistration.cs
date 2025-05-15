using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shosh.Data.IRepository;
using Shosh.Data;
using Shosh.Data.Migrations;
using Shosh.Data.Repository;
using Shosh.Service.IService;


namespace Shosh.Service.Service
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddShoshServices(this IServiceCollection services, IConfiguration configuration)
        {
            // PostgreSQL Bağlantısı
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Servisleri kaydet
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            

            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IEntryService, EntryService>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITopicService, TopicService>();

            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<ILikeService, LikeService>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IUserFollowService, UserFollowService>();
            services.AddScoped<IUserFollowRepository, UserFollowRepository>();

            services.AddScoped<IComplaintService, ComplaintService>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();

            services.AddScoped<IBanRepository, BanRepository>();
            services.AddScoped<IBanService, BanService>();
            services.AddHostedService<BanCleanupService>();


            services.AddScoped<IBlogRepository, BlogRepository>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
