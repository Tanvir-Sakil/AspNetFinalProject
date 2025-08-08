using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class PaymentItemDto
    {
        public Guid PaymentID { get; set; }

        public string AccountNo { get; set; }

        public string AccountType { get; set; }

        public string Note { get; set; }
    }
}
