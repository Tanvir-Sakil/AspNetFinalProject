using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class CustomerLedger :IEntity<Guid>
    {
        public Guid Id { get; set; } 

        public Guid CustomerId { get; set; } 
        public DateTime Date { get; set; } 

        public string? InvoiceNo { get; set; }

        public string Particulars { get; set; }

        public decimal Total { get; set; } = 0;    
        public decimal Discount { get; set; } = 0;
        public decimal Vat { get; set; } = 0;     
        public decimal Paid { get; set; } = 0;      
        public decimal Balance { get; set; } = 0;   

        public string SourceType { get; set; } 
        public Guid? SourceId { get; set; }     

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public  Customer Customer { get; set; }
    }

}
