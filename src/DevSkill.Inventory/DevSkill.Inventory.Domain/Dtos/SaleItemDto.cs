using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class SaleItemDto
    {
        public Guid ProductID { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double ReturnedQuantity { get; set; }

        public double ReturnableQty { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }

       
    }

}
