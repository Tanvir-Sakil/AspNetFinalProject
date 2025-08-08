using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class CustomerSearchDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int? RatingFrom { get; set; }

        public int? RatingTo { get; set; }
    }
}
