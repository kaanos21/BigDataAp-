using BigDataApı.Context;
using BigDataApı.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BigDataApı.Repositories.Concrete
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly BigDataOrdersDbContext _context;

        public StatisticsRepository(BigDataOrdersDbContext context)
        {
            _context = context;
        }

        public async Task<double> GetAverageProductPrice()
        {
            return await _context.Products.AverageAsync(x => x.UnitPrice);
        }
        public async Task<double> GetAverageProductQuantity()
        {
            return await _context.Products.AverageAsync(x => x.StockQuantity);
        }
        public async Task<string> GetLastAddedCustomerFullNameAsync()
        {
            return await _context.Customers
                      .AsNoTracking()
                      .OrderByDescending(c => c.CustomerId)
                      .Select(c => c.CustomerName + " " + c.CustomerSurname)
                      .FirstOrDefaultAsync();
        }
        public async Task<string> GetLeastExpensiveProductNameAsync()
        {
            var productName = await _context.Products
                      .AsNoTracking()
                      .OrderBy(p => p.UnitPrice)
                      .Select(p => p.ProductName)
                      .FirstOrDefaultAsync();
            return productName;
        }
        public async Task<string> GetLeastSoldProductAsync()
        {
            var product = await _context.Orders
                .AsNoTracking()
                .GroupBy(o => o.ProductId)
                .OrderBy(g => g.Sum(o => o.Quantity))
                .Select(g => new
                {

                    Name = g.FirstOrDefault().Product.ProductName
                })
                .FirstOrDefaultAsync();

            return product?.Name ?? "Veri Yok";
        }
        public async Task<string> GetMostExpensiveProductNameAsync()
        {
            var productName = await _context.Products
                      .AsNoTracking()                  
                      .OrderByDescending(p => p.UnitPrice) 
                      .Select(p => p.ProductName)      
                      .FirstOrDefaultAsync();
            return productName;
        }
        public Task<string> GetMostPaymantMethodAsync()
        {
            return _context.Orders
                      .AsNoTracking()
                      .GroupBy(o => o.PaymentMethod)
                      .OrderByDescending(g => g.Count())
                      .Select(g => g.Key)
                      .FirstOrDefaultAsync();
        }

        public async Task<string> GetMostSellCategoryNameAsync()
        {
            var product=await _context.Orders
                .AsNoTracking()
                .GroupBy(o => o.Product.Category.CategoryId) 
                .OrderByDescending(g => g.Sum(o => o.Quantity)) 
                .Select(g => new
                {
                    
                    Name = g.FirstOrDefault().Product.Category.CategoryName
                })
                .FirstOrDefaultAsync();
            return product?.Name ?? "Kategori Bulunamadı";
        }

        public async Task<string> GetMostSellProductCountryNameAsync()
        {
            var country = await _context.Orders
                .GroupBy(o => o.Customer.CustomerCountry) 
                .OrderByDescending(g => g.Count())       
                .Select(g => g.Key)                  
                .FirstOrDefaultAsync();

            return country;
        }
        public async Task<string> GetMostSoldProductAsync()
        {
            var product = await _context.Orders
                .AsNoTracking()
                .GroupBy(o => o.ProductId) 
                .OrderByDescending(g => g.Sum(o => o.Quantity)) 
                .Select(g => new
                {
                    
                    Name = g.FirstOrDefault().Product.ProductName
                })
                .FirstOrDefaultAsync();

            return product?.Name ?? "Veri Yok";
        }
        public async Task<string> GetMostStockProductNameAsync()
        {
            return await _context.Products
                      .AsNoTracking()
                      .OrderByDescending(p => p.StockQuantity)
                      .Select(p => p.ProductName)
                      .FirstOrDefaultAsync();
        }
        public async Task<int> GetTotalCategoryCountAsync()
        {
            return await _context.Categories.CountAsync();
        }
        public async Task<int> GetTotalCustomerCityCountAsync()
        {
            return await _context.Customers.Select(c => c.CustomerCity).Distinct().CountAsync();
        }
        public async Task<int> GetTotalCustomerCountAsync()
        {
            return await _context.Customers.CountAsync();
        }
        public async Task<int> GetTotalCustomerCountryCountAsync()
        {
            return await _context.Customers.Select(c=> c.CustomerCountry).Distinct().CountAsync();
        }
        public async Task<int> GetTotalOrderCountAsync()
        {
            return await _context.Orders.CountAsync();
        }
        public async Task<int> GetTotalOrderStatusByCancelled()
        {
            return await _context.Orders.CountAsync(o => o.OrderStatus == "İptal Edildi");
        }
        public async Task<int> GetTotalOrderStatusByCompleted()
        {
            return await _context.Orders.CountAsync(o => o.OrderStatus == "Tamamlandı");
        }
        public async Task<int> GetTotalOrdersİnOctober2025()
        {
            DateTime start = new DateTime(2025, 10, 1, 0, 0, 0);
            DateTime end = new DateTime(2025, 10, 31, 23, 59, 59);

            return await _context.Orders
                .Where(x => x.OrderDate >= start && x.OrderDate <= end)
                .CountAsync();
        }
        public async Task<int> GetTotalProductCountAsync()
        {
            return await _context.Products.CountAsync();
        }

    }
}
