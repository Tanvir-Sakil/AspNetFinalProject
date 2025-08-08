using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class GetMobileAccountByIdQuery : IRequest<Domain.Entities.MobileAccount>
    {
        public Guid Id { get; set; }
    }
}
