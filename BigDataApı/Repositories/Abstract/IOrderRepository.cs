using BigDataApı.Entities;
using BigDataApi.Repositories.Concrete;

namespace BigDataApi.Repositories.Abstract;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> OrderListWithPaging(int page, int pageSize);
}