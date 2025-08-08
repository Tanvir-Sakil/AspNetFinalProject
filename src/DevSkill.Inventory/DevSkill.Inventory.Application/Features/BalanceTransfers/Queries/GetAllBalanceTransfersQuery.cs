using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Domain.Features.Sales.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Queries
{
    public class GetAllBalanceTransferQuery : DataTables, IRequest<(IList<BalanceTransferDto>, int, int)>, IGetBalanceTransferQuery
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
