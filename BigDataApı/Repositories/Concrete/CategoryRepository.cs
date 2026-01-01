using BigDataApı.Context;
using BigDataApı.Entities;
using BigDataApi.Repositories.Abstract;

namespace BigDataApi.Repositories.Concrete;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(BigDataOrdersDbContext context) : base(context)
    {

    }
}