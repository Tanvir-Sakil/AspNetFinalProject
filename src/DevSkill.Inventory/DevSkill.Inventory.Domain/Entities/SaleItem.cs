using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => (decimal)Quantity * UnitPrice;
        public double ReturnedQuantity { get; set; }
        public Sale Sale { get; set; }    
        public Product Product { get; set; } 
    }

}
