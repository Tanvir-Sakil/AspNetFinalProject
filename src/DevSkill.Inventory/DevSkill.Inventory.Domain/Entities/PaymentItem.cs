using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class PaymentItem
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }

        public string Note { get; set; }
        public Sale Sale { get; set; }

    }

}
