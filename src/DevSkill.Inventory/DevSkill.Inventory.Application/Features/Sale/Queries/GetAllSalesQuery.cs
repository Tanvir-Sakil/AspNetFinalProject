using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Features.Sales.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Sale.Queries
{
    public class GetAllSalesQuery : DataTables, IRequest<(IList<SaleDto>, int, int)>, IGetSalesQuery
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
