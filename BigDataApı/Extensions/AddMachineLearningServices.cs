using Microsoft.ML;
using BigDataApı.ML.NET.Abstract;
using BigDataApı.ML.NET.Concrete;

public static class MachineLearningExtensions
{
    public static void AddMachineLearningServices(this IServiceCollection services)
    {
        services.AddSingleton<MLContext>();

        services.AddScoped<IPredictionService, PredictionService>();
        services.AddScoped<ILoyaltyService, LoyaltyService>();
    }
}