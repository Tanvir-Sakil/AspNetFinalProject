using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Users.Commands
{
    public class AddUserCommand : IRequest<bool>
    {
        public Guid EmployeeId { get; set; }
        public Guid RoleId { get; set; }
        public string Password { get; set; }
    }
}
