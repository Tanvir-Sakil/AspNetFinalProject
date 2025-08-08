using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Enums;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Sale :IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }   
        public DateTime Date { get; set; }
        public Guid CustomerID { get; set; }
        public Guid SalesTypeId { get; set; }
        public string Terms { get; set; }
        public decimal VAT { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public Customer Customer { get; set; }

        public SaleType SaleType { get; set; }

        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

        public ICollection<PaymentItem> PaymentItems { get; set; } = new List<PaymentItem>();
    }


}
