namespace BigDataApı.Repositories.Abstract
{
    public interface IReviewService
    {
        Task<string> GetCustomerReviewWithOpenAIAnalysis();
    }
}
