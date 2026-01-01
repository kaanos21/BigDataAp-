using BigDataApı.Context;
using BigDataApı.Entities;
using BigDataApi.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BigDataApi.Repositories.Concrete;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(BigDataOrdersDbContext context) : base(context) 
    {

    }

    public async Task<object> ProductListWithCategoryAndPaging(int page, int pageSize)
    {
        var values = await _context.Products
            .AsNoTracking()                  
            .OrderBy(p => p.ProductId)       
            .Skip((page - 1) * pageSize)     
            .Take(pageSize)                 
            .Select(p => new                 
            {
                p.ProductId,
                p.ProductName,
                p.ProductDescription,
                p.UnitPrice,
                p.StockQuantity,
                p.CountryOfOrigin,
                p.ProductImageUrl,
                CategoryName = p.Category.CategoryName
            })
            .ToListAsync();

        return values;
    }

    public async Task<List<Product>> ProductListWithPaging(int page, int pageSize)
    {
        var values = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return values;
    }
}