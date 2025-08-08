using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class SaleType:IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string PriceName { get; set; }
    }

}
