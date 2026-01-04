using BigDataApý.Context;
using BigDataApi.Extensions;
using BigDataApý.ML.NET.Abstract;
using BigDataApý.ML.NET.Concrete;
using BigDataApi.Repositories.Abstract;
using BigDataApý.Repositories.Abstract;
using BigDataApi.Repositories.Concrete;
using BigDataApý.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<BigDataOrdersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<MLContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
builder.Services.AddScoped<IPredictionService,PredictionService>();
builder.Services.AddScoped<ICustomerAnalyticsService, CustomerAnalyticsService>();
builder.Services.AddScoped<IOrderAnalyticsService, OrderAnalyticsService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
