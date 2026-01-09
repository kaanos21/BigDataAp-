using BigDataApı.Context;
using BigDataApı.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BigDataApı.Repositories.Concrete
{
    public class OrderAnalyticsService : IOrderAnalyticsService
    {
        private readonly BigDataOrdersDbContext _context;

        public OrderAnalyticsService(BigDataOrdersDbContext context)
        {
            _context = context;
        }

        public Task<object> GetCategoryOrderCountsByYear()
        {
            // veriyi kategorileyerek çekme
            var categoryData = _context.Orders
                .Include(o => o.Product)
                .ThenInclude(p => p.Category)
                .AsEnumerable()
                .GroupBy(o => new
                {
                    Year = o.OrderDate.Year,
                    CategoryName = o.Product.Category.CategoryName
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.CategoryName,
                    OrderCount = g.Count()
                })
                .OrderBy(data => data.Year)
                .ToList();

            // her bir kategorinin toplam satış sayısı

            var topCategories=categoryData
                .GroupBy(data => data.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalOrders = g.Sum(x => x.OrderCount)
                })
                .OrderByDescending(x => x.TotalOrders)
                .Take(5)
                .Select(x => x.CategoryName)
                .ToList();

            //Sdece bu 5 kategoriye ait verilerin listesi

            var filteredData = categoryData
                .Where(data => topCategories.Contains(data.CategoryName))
                .ToList();

            var years= filteredData
                .Select(data => data.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            var chartSeries=topCategories.Select(category => new
            {
                CategoryName = category,
                data=years.Select(y=> filteredData.FirstOrDefault(d=>d.CategoryName == category && d.Year==y)?.OrderCount ?? 0).ToList()
            }).ToList();

            return Task.FromResult<object>(new
            {
                Years = years,
                Series = chartSeries
            });
        }

        public Task<object> GetItalyLoyaltyScoreItalyWithOutML()
        {
            var loyaltScores = _context.Customers
                .Include(o => o.Orders)
                .ThenInclude(o => o.Product)
                .Where(c => c.CustomerCountry == "İtalya")
                .Select(c => new
                {
                    CustomerName = c.CustomerName + " " + c.CustomerSurname,
                    TotalOrders = c.Orders.Count,
                    TotalSpent = c.Orders.Sum(o => o.Quantity * o.Product.UnitPrice),
                    LastOrderDate = c.Orders.Max(o => (DateTime?)o.OrderDate),
                })
                .AsEnumerable()
                .Select(x =>
                {
                    var daysSinceLastOrder = x.LastOrderDate.HasValue ? (DateTime.Now - x.LastOrderDate.Value).TotalDays : double.MaxValue;

                    double recencryScore = daysSinceLastOrder switch
                    {
                        <= 30 => 100,
                        <= 90 => 80,
                        <= 180 => 50,
                        <= 365 => 40,
                        _ => 10
                    };

                    double frequencyScore = x.TotalOrders switch
                    {
                        >= 20 => 100,
                        >= 10 => 80,
                        >= 5 => 60,
                        >= 2 => 40,
                        >= 1 => 20,
                        _ => 10
                    };

                    double monetaryScore = x.TotalSpent switch
                    {
                        >= 5000 => 100,
                        >= 2000 => 80,
                        >= 1000 => 60,
                        >= 500 => 40,
                        >= 100 => 20,
                        _ => 10
                    };

                    double loyaltyScore = (recencryScore * 0.4) + (frequencyScore * 0.3) + (monetaryScore * 0.3);

                    return new
                    {
                       CustomerName= x.CustomerName,
                       TotalOrders=x.TotalOrders,
                       TotalSpent=Math.Round(x.TotalSpent,2),
                       LastOrderDate=x.LastOrderDate,
                       LoyaltyScore = Math.Round(loyaltyScore, 2)
                    };
                })
                .OrderByDescending(x => x.LoyaltyScore)
                .ToList();
            return Task.FromResult<object>(loyaltScores);
        }

        public async Task<object> GetOrdersPerCity()
        {
            var data =await _context.Orders
                .Include(x => x.Customer)
                .Where(x => x.Customer.CustomerCity != null)
                .GroupBy(o => o.Customer.CustomerCountry)
                .Select(g => new
                {
                    Country = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(5)
                .ToListAsync();
            return data;
        }
    }
}
