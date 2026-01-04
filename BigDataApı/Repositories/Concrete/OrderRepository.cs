using BigDataApı.Context;
using BigDataApı.Entities;
using BigDataApi.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BigDataApi.Repositories.Concrete;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(BigDataOrdersDbContext context) : base(context)
    {
    }

    public async Task<List<Order>> OrderListWithPaging(int page, int pageSize)
    {
        var values = await _context.Orders
            .AsNoTracking()
            .OrderBy(p => p.OrderId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return values;
    }
}