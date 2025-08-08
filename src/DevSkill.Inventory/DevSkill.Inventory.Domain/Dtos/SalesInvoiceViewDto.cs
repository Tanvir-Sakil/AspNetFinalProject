using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Enums;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class SalesInvoiceViewDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
