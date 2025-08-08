using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class CustomerLedgerViewDto
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public int ReportYear { get; set; }
        public List<CustomerLedger> Transactions { get; set; }
    }
}
