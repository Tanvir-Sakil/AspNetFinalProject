using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class SearchMobileAccountQuery : IRequest<List<SearchMobileAccountDto>>
    {
        public string Query { get; set; } = string.Empty;
    }

}
