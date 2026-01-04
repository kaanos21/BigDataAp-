namespace BigDataApı.Repositories.Abstract
{
    public interface IStatisticsRepository
    {
        Task<int> GetTotalCategoryCountAsync();
        Task<int> GetTotalCustomerCountAsync();
        Task<int> GetTotalProductCountAsync();
        Task<int> GetTotalOrderCountAsync();
        Task<int> GetTotalCustomerCountryCountAsync();
        Task<int> GetTotalCustomerCityCountAsync();
        Task<int> GetTotalOrderStatusByCompleted();
        Task<int> GetTotalOrderStatusByCancelled();
        Task<int> GetTotalOrdersİnOctober2025();
        Task<double> GetAverageProductPrice();
        Task<double> GetAverageProductQuantity();
        Task<string> GetMostExpensiveProductNameAsync();
        Task<string> GetLeastExpensiveProductNameAsync();
        Task<string> GetMostStockProductNameAsync();
        Task<string> GetLastAddedCustomerFullNameAsync();
        Task<string> GetMostPaymantMethodAsync();
        Task<string> GetMostSoldProductAsync();
        Task<string> GetLeastSoldProductAsync();
        Task<string> GetMostSellProductCountryNameAsync();
        Task<string> GetMostSellCategoryNameAsync();
    }
}
