using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{ 
    public class Customer : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string CustomerID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal OpeningBalance { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? ImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public List<Product> Products { get; set; }

        public List<Sale> Sales { get; set; }


    }
}
