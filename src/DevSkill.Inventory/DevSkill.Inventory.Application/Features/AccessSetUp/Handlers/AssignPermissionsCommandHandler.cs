using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.AccessSetUp.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.AccessSetUp.Handlers
{
    public class AssignPermissionsCommandHandler : IRequestHandler<AssignPermissionsCommand, Unit>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public AssignPermissionsCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AssignPermissionsCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.UserPermissionRepository;
        //    await repo.RemoveAllPermissionsForUserAsync(request.UserId);
            foreach (var permission in request.Permissions)
            {
                await repo.AddPermissionToUserAsync(request.UserId, permission);
            }
            return Unit.Value;
        }
    }
}
