namespace BigDataApı.Repositories.Abstract
{
    public interface ICustomerAnalyticsService
    {
        Task<int> GetTotalCustomerCount();
        Task<double> GetAveragelOrdersCountPerCustomer();
        Task<int> GetTotalActiveCustomerIn3Month();

        Task<int> GetTotalDeactiveCustomerIn6Month();
        Task<string> GetCustomerDetailAlAnalysisByLastOrderOldMethod(int id);
        Task<string> GetCustomerDetailAlAnalysisByLastOrderModernMethod(int id);
    }
}
