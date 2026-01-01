using BigDataApi.Repositories.Abstract;
using BigDataApı.Entities;

namespace BigDataApı.Repositories.Abstract
{
    public interface ICustomerRepository :IRepository<Customer>
    {
        Task<List<Customer>> CustomerListWithPaging(int page, int pageSize);
    }
}
