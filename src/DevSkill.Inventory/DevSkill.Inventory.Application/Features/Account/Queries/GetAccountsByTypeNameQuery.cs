using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Account.Queries
{
    public class GetAccountsByTypeNameQuery : IRequest<List<AccountTypeDto>>
    {
        public string AccountTypeName { get; set; }
    }
}
