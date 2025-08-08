using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Commands
{
    public class UserRoleDeleteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
