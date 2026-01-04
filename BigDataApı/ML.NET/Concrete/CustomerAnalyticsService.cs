using BigDataApı.Context;
using BigDataApı.ML.NET.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace BigDataApı.ML.NET.Concrete
{
    public class CustomerAnalyticsService: ICustomerAnalyticsService
    {
        private readonly BigDataOrdersDbContext _context;
        private readonly MLContext _mLContext;

        public CustomerAnalyticsService(BigDataOrdersDbContext context, MLContext mLContext)
        {
            _context = context;
            _mLContext = mLContext;
        }

        public async Task<double> GetAveragelOrdersCountPerCustomer()
        {
            var value=await _context.Orders
                .GroupBy(o => o.CustomerId)
                .Select(g => g.Count())
                .AverageAsync();
            return (double)value;
        }

        public async Task<int> GetTotalActiveCustomerIn3Month()
        {
            var value = await _context.Orders
                .Where(o => o.OrderDate >= DateTime.Now.AddMonths(-3))
                .Select(o => o.CustomerId)
                .Distinct()
                .CountAsync();
            return value;
        }

        public async Task<int> GetTotalCustomerCount()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<int> GetTotalDeactiveCustomerIn6Month()
        {
            var value =await _context.Customers
                .Where(c => !_context.Orders
                    .Any(o => o.CustomerId == c.CustomerId && o.OrderDate >= DateTime.Now.AddMonths(-6)))
                .CountAsync();
            return value;
        }
    }
}
