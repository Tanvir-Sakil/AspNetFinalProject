using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CustomerLedgerRepository : Repository<CustomerLedger,Guid>,ICustomerLedgerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerLedgerRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<List<CustomerLedger>> GetCustomerLedgerAsync(Guid customerId, string reportType,
            DateTime? start, DateTime? end, int? month, int? year, int? reportYear)
        {
            var query = _context.CustomerLedgers.Where(x => x.CustomerId == customerId);

            if (reportType == "dailyReports" && start.HasValue && end.HasValue)
                query = query.Where(x => x.Date >= start && x.Date <= end);
            else if (reportType == "monthlyReports" && month.HasValue && year.HasValue)
                query = query.Where(x => x.Date.Month == month && x.Date.Year == year);
            else if (reportType == "yearlyReports" && reportYear.HasValue)
                query = query.Where(x => x.Date.Year == reportYear);

            return await query
                .OrderBy(x => x.Date)
                .Select(x => new CustomerLedger
                {
                    Date = x.Date,
                    InvoiceNo = x.InvoiceNo,
                    Particulars = x.Particulars,
                    Total = x.Total,
                    Discount = x.Discount,
                    Vat = x.Vat,
                    Paid = x.Paid,
                    Balance = x.Balance
                })
                .ToListAsync();
        }

        public async Task<decimal> GetLastBalanceAsync(Guid customerId)
        {
            return await _context.CustomerLedgers
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.Date)
                .Select(x => x.Balance)
                .FirstOrDefaultAsync();
        }
    }

}

