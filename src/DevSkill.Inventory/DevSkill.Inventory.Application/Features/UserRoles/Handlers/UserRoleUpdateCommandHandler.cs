using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Handlers
{
    public class UserRoleUpdateCommandHandler : IRequestHandler<UserRoleUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public UserRoleUpdateCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UserRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var userRole = await _unitOfWork.UserRoleRepository.GetRoleByIdAsync(request.Id);

            if (userRole == null)
                return false;

            userRole.Update(request.RoleName, request.IsActive);

            await _unitOfWork.UserRoleRepository.UpdateRoleAsync(userRole);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
