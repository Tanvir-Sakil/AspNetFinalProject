using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class CashAccount : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public bool IsActive { get; set; }

        public decimal Balance { get; set; }

        public decimal CurrentBalance { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
