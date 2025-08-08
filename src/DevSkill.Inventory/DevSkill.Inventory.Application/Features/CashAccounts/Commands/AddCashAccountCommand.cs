using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Commands
{
    public class AddCashAccountCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }=true;

        public decimal Balance { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}
