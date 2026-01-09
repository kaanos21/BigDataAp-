namespace BigDataApı.Repositories.Abstract
{
    public interface IOrderAnalyticsService
    {
        Task<object> GetCategoryOrderCountsByYear();

        Task<object> GetOrdersPerCity();

        Task<object> GetItalyLoyaltyScoreItalyWithOutML();
    }
}
