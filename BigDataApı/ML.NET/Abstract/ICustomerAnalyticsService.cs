namespace BigDataApı.ML.NET.Abstract
{
    public interface ICustomerAnalyticsService
    {
        Task<int> GetTotalCustomerCount();
        Task<double> GetAveragelOrdersCountPerCustomer();
        Task<int> GetTotalActiveCustomerIn3Month();

        Task<int> GetTotalDeactiveCustomerIn6Month();
    }
}
