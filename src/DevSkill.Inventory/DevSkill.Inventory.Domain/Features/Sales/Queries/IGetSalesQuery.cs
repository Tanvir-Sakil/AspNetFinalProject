using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Features.Sales.Queries
{
    public interface IGetSalesQuery : IDataTables
    {
        public string? CustomerName { get; set; }
        public decimal? TotalAmountFrom { get; set; }
        public decimal? TotalAmountTo { get; set; }
        public decimal? PaidAmountFrom { get; set; }
        public decimal? PaidAmountTo { get; set; }
        public decimal? DueAmountFrom { get; set; }
        public decimal? DueAmountTo { get; set; }

        public string? OrderBy { get; set; }

    }
}
