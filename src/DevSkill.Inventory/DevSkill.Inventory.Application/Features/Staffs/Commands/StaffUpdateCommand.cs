using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Application.Features.Staffs.Commands
{
    public class UpdateStaffCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public string StaffCode { get; set; }

        public string? EmployeeName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public decimal? Salary { get; set; }

        public DateTime JoiningDate { get; set; }

        public string? NID { get; set; }

        public bool IsActive { get; set; }
    }
 }
