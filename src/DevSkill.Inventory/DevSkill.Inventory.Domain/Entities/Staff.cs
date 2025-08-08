using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Staff :IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string StaffCode { get; set; }
        public string EmployeeName { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal? Salary { get; set; }
        public string Nid { get; set; }
        public bool IsActive { get; set; } = true;

        public Department Department { get; set; }
    }

}
