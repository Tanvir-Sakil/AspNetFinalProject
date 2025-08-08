using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Commands
{
    public class AddDepartmentCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }=true;
    }
}
