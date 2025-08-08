using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Queries
{
    public class SearchCashAccountQuery : IRequest<List<SearchCashAccountDto>>
    {
        public string Query { get; set; } = string.Empty;
    }

}
