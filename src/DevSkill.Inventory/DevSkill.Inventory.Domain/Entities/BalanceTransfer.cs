using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class BalanceTransfer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string FromAccountType { get; set; } 
        public Guid FromAccountId { get; set; }

        public string ToAccountType { get; set; }
        public Guid ToAccountId { get; set; }

        public decimal Amount { get; set; }
        public string Note { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;
    }

}
