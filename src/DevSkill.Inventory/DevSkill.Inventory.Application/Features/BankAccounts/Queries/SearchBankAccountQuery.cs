using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Queries
{
    public class SearchBankAccountQuery : IRequest<List<SearchBankAccountDto>>
    {
        public string Query { get; set; } = string.Empty;
    }

}
