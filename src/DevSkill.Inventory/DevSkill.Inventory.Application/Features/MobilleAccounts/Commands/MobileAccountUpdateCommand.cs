using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Commands
{
    public class MobileAccountUpdateCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }

        public string AccountNo { get; set; }

        public string OwnerName { get; set; }

        public bool IsActive { get; set; }

        public decimal Balance { get; set; }

        public decimal CurrentBalance { get; set; }
    }


}
