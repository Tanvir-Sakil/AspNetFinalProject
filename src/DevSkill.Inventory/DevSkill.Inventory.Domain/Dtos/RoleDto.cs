using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string? CompanyName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
