using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Queries
{
    public class GetCashAccountByIdQuery : IRequest<Domain.Entities.CashAccount>
    {
        public Guid Id { get; set; }
    }
}
