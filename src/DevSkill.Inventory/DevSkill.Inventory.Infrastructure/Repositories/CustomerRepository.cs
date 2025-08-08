using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Features.Units.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer, Guid>, ICustomerRepository
    {
        public readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public (IList<Customer> data, int total, int totalDisplay) GetPagedCustomers(int pageIndex,
           int pageSize, string? order, DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Name.Contains(search.Value) ||
                //x.Description.Contains(search.Value),
                true,
                order, null, pageIndex, pageSize, true);
        }

        public async Task<(IList<Customer> Data, int Total, int TotalDisplay)> GetPagedCustomerAsync(IGetCustomersQuery request)
        {
            Expression<Func<Customer, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                filter = filter.AndAlso(c => c.Name.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                filter = filter.AndAlso(c => c.Email.Contains(request.Email));
            }

            if (request.BalanceFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.OpeningBalance >= request.BalanceFrom.Value);
            }
            if (request.BalanceTo > 0)
            {
                filter = filter.AndAlso(c => c.OpeningBalance <= request.BalanceTo);
            }
            if (!string.IsNullOrWhiteSpace(request.Search.Value))
            {
                string searchTerm = request.Search.Value.Trim();

                filter = filter.AndAlso(c =>
                    c.Name.Contains(searchTerm) ||
                    c.Email.Contains(searchTerm) ||
                    c.OpeningBalance.ToString().Contains(searchTerm) ||
                    c.CreatedDate.ToString().Contains(searchTerm)
                );
            }

            return await GetDynamicAsync(
                filter,
                request.OrderBy,
                include: null,
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                isTrackingOff: true
            );
        }


        public bool IsNameDuplicate(string name, Guid? id = null)
        {
            if (id.HasValue)
                return GetCount(x => x.Id != id.Value && x.Name == name) > 0;
            else
                return GetCount(x => x.Name == name) > 0;
        }

        public IList<Customer> SearchByName(string query, int limit = 20)
        {
            return _dbContext.Set<Customer>()
                .Where(c => c.Name.Contains(query))
                .OrderBy(c => c.Name)
                .Take(limit)
                .ToList();
        }

        public async Task<Customer?> GetByNameAsync(string name)
        {
            return await _dbContext.Customers
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
        }

        public async Task<string> GenerateNextCustomerCodeAsync()
        {
            var lastCode = await _dbContext.Customers
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => c.CustomerID)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith("C-INV"))
            {
                var numberPart = lastCode.Substring(6);
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"C-INV{nextNumber:D5}";
        }

        public async Task<Customer?> GetCustomerByIdWithSalesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Customers
                .Include(c => c.Sales)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }


    }
}
