using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class CustomerDropDownDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
