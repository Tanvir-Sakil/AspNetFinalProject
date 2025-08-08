using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Staffs.Commands
{
    public class AddStaffCommand : IRequest<Guid>
    {
        public string StaffCode { get; set; }
        public string EmployeeName { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal? Salary { get; set; }
        public string Nid { get; set; }
    }

}
