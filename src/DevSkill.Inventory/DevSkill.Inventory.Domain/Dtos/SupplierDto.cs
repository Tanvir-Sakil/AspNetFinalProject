using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string Mobile { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}
