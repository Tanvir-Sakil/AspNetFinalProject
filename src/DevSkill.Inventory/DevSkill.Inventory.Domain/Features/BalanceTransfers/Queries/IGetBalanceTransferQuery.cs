using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Features.BalanceTransfers.Queries
{
    public interface IGetBalanceTransferQuery :IDataTables
    {
        public string? FromAccountName { get; set; }

        public string? ToAccountName { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal? AmountFrom { get; set; }

        public decimal? AmountTo { get; set; }

        public string? OrderBy { get; set; }
    }
}
