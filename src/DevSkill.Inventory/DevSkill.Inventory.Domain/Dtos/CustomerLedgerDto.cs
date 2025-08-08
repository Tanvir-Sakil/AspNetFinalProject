using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class CustomerLedgerDto
    {
        public DateTime Date { get; set; }
        public string Invoice { get; set; }
        public string Particulars { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
    }
}
