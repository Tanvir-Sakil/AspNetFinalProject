using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Sale.Commands
{
    public class PaymentAddCommand : IRequest<bool>
    {
        public Guid SaleID { get; set; }

        public decimal DueAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public string AccountNo { get; set; }


        public string AccountType { get; set; }

        public string Note { get; set; }
    }
}
