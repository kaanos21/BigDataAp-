using BigDataApı.Entities;
namespace BigDataApi.Repositories.Abstract;

public interface IProductRepository : IRepository<Product> 
{
    Task<List<Product>> ProductListWithPaging(int page, int pageSize);
    Task<object> ProductListWithCategoryAndPaging(int page, int pageSize);
}