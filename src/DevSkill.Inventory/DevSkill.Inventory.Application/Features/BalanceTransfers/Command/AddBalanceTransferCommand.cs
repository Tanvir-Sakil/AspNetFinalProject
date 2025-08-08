using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Command
{
    public class AddBalanceTransferCommand : IRequest<bool>
    {
        public string FromAccountType { get; set; }
        public Guid FromAccountId { get; set; }
        public string ToAccountType { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }

}
