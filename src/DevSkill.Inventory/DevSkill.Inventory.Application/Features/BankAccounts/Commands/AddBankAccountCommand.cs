using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Commands
{
    public class AddBankAccountCommand : IRequest<bool>
    {
        public string AccountName { get; set; }

        public string AccountNo { get; set; }

        public string BankName { get; set; }

        public string BranchName { get; set; }

        public bool IsActive { get; set; }=true;

        public decimal OpeningBalance { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}
