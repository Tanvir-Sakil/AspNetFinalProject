using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.AccessSetUp.Commands
{
    public record AssignPermissionsCommand(Guid UserId, List<string> Permissions) : IRequest<Unit>;
}
