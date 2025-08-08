using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.BalanceTransfers.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IBalanceTransferRepository:IRepository<BalanceTransfer, Guid>
    {
        Task<(IList<BalanceTransfer> Data, int Total, int TotalDisplay)> GetAllBalanceTransferAsync(IGetBalanceTransferQuery request);
    }
}
