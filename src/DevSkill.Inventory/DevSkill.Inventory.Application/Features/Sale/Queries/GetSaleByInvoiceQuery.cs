using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Sale.Queries
{
    public record GetSaleByInvoiceQuery(string Invoice) : IRequest<SaleDto>;
}
