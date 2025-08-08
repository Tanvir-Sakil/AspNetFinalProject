using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Commands
{
    public class CashAccountUpdateCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public decimal Balance { get; set; }

        public decimal CurrentBalance { get; set; }
    }


}
