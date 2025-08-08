using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class SearchMobileAccountDto
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }

        public string AccountNo { get; set; }

        public string MobileName { get; set; }

        public string BranchName { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
