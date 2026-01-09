using BigDataApı.Context;
using BigDataApi.Repositories.Abstract;
using BigDataApı.Repositories.Abstract;
using BigDataApi.Repositories.Concrete;
using BigDataApı.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BigDataApı.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Veritabanı Bağlantısı (Context)
            services.AddDbContext<BigDataOrdersDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<ICustomerAnalyticsService, CustomerAnalyticsService>();
            services.AddScoped<IOrderAnalyticsService, OrderAnalyticsService>();
            services.AddScoped<IReviewService, ReviewService>();

            services.AddHttpClient();
        }
    }
}