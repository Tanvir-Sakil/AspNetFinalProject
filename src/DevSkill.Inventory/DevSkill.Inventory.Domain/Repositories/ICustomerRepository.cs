using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Customers.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer,Guid>
    {
        bool IsNameDuplicate(string name, Guid? id = null);
        //(IList<Customer> data, int total, int totalDisplay) GetPagedCustomers(int pageIndex,
        //    int pageSize, string? order, DataTablesSearch search);

        Task<(IList<Customer> Data, int Total, int TotalDisplay)>GetPagedCustomerAsync(IGetCustomersQuery request);

        void Update(Customer customer);

        IList<Customer> SearchByName(string query, int limit = 20);

        Task<Customer?> GetByNameAsync(string name);

        Task<string> GenerateNextCustomerCodeAsync();

        Task<Customer> GetCustomerByIdWithSalesAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
