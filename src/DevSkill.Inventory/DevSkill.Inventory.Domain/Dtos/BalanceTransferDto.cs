using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class BalanceTransferDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}
