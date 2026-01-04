namespace BigDataApı.ML.NET.Abstract
{
    public interface IPredictionService
    {
        Task<object> GetPaymentMethodForecastWith2025To2026Q1Async();
        Task<object> GetGermanyCitiesSalesForecast();
    }
}
