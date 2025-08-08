using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Features.UserRoles.Queries
{
    public interface IGetUserRoleListQuery :IDataTables
    {
        public string? RoleName { get; set; }
        public string? CompanyName { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDateFrom  { get; set; }


        public DateTime? CreateDateTo { get; set; }

        public string? OrderBy { get; set; }
    }
}
