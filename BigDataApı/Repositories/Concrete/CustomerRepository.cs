using BigDataApı.Context;
using BigDataApı.Entities;
using BigDataApı.Repositories.Abstract;
using BigDataApi.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BigDataApı.Repositories.Concrete
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(BigDataOrdersDbContext context) : base(context)
        {
        }

        public async Task<List<Customer>> CustomerListWithPaging(int page, int pageSize)
        {
            var values = await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return values;
        }
    }
}
