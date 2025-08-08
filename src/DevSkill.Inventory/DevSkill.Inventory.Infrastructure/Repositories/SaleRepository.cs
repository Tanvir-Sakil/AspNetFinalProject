using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Features.Sales.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
  

    public class SalesRepository : Repository<Sale,Guid>,ISalesRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesRepository(ApplicationDbContext context): base(context) 
        {
            _context = context;
        }

        public async Task<Sale> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleType)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale> GetByInvoiceWithDetailsAsync(string invoice)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleType)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.InvoiceNo == invoice);
        }

        public async Task<string> GenerateInvoiceAsync()
        {
            var lastInvoice = await _context.Sales
                .OrderByDescending(s => s.InvoiceNo)
                .Select(s => s.InvoiceNo)
                .FirstOrDefaultAsync();

            int lastNumber = 0;
            if (!string.IsNullOrEmpty(lastInvoice) && lastInvoice.StartsWith("INV-"))
            {
                int.TryParse(lastInvoice.Substring(4), out lastNumber);
            }

            return $"INV-{(lastNumber + 1).ToString("D5")}";
        }



        public async Task AddSaleAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }

        public async Task<(IList<Sale> Data, int Total, int TotalDisplay)> GetAllSaleAsync(IGetSalesQuery request)
        {


            Expression<Func<Sale, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.CustomerName))
            {
                filter = filter.AndAlso(c => c.Customer.Name.Contains(request.CustomerName));
            }
            if (request.TotalAmountFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.TotalAmount >= request.TotalAmountFrom.Value);
            }

            if (request.TotalAmountTo.HasValue)
            {
                filter = filter.AndAlso(c => c.TotalAmount <= request.TotalAmountTo.Value);
            }

            if (request.PaidAmountFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.PaidAmount >= request.PaidAmountFrom.Value);
            }

            if (request.PaidAmountTo.HasValue)
            {
                filter = filter.AndAlso(c => c.PaidAmount <= request.PaidAmountTo.Value);
            }

            if (request.DueAmountFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.DueAmount >= request.DueAmountFrom.Value);
            }

            if (request.DueAmountTo.HasValue)
            {
                filter = filter.AndAlso(c => c.DueAmount <= request.DueAmountTo.Value);
            }

            //if (!string.IsNullOrWhiteSpace(request.InvoiceNo))
            //{
            //    string searchTerm = request.InvoiceNo.Trim();

            //    filter = filter.AndAlso(c =>
            //        c.InvoiceNo.Contains(searchTerm)
            //    //  c.IsActive.ToString().Contains(searchTerm) ||
            //    // c.CreatedDate.ToString().Contains(searchTerm)
            //    );
            //}
            return await GetDynamicAsync(
                filter,
                request.OrderBy,
                q => q.Include(p => p.Customer),
                request.PageIndex,
                request.PageSize,
                isTrackingOff: true
            );
        }
    }

}
