using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? ImagePath { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MRPPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public int Stock { get; set; }
        public int LowStock { get; set; }
        public int DamageStock { get; set; }

        public string? Unit { get; set; }
        public decimal? UnitPrice { get; set; }
      
    }

}
