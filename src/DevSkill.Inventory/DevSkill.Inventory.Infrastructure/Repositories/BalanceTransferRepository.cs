using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class BalanceTransferRepository : Repository<BalanceTransfer, Guid>,IBalanceTransferRepository
    {
        private readonly ApplicationDbContext _context;

        public BalanceTransferRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<(IList<BalanceTransfer> Data, int Total, int TotalDisplay)> GetAllBalanceTransferAsync(IGetBalanceTransferQuery request)
        {
            Expression<Func<BalanceTransfer, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.FromAccountName))
            {
                filter = filter.AndAlso(c => c.FromAccountType.Contains(request.FromAccountName));
            }

            if (!string.IsNullOrWhiteSpace(request.ToAccountName))
            {
                filter = filter.AndAlso(c => c.ToAccountType.Contains(request.ToAccountName));
            }

            if (request.DateFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.TransferDate >= request.DateFrom.Value);
            }

            if (request.DateTo.HasValue)
            {
                filter = filter.AndAlso(c => c.TransferDate <= request.DateTo.Value);
            }

            if (request.AmountFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.Amount >= request.AmountFrom.Value);
            }

            if (request.AmountTo.HasValue)
            {
                filter = filter.AndAlso(c => c.Amount <= request.AmountTo.Value);
            }

            var (data, total, totalDisplay) = await GetDynamicAsync(
                filter,
                request.OrderBy,
                null,
                request.PageIndex,
                request.PageSize,
                isTrackingOff: true
            );

            return (data, total, totalDisplay);
        }

    }
}
