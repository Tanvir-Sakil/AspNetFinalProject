using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Handlers
{
    public class UserRoleDeleteCommandHandler : IRequestHandler<UserRoleDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public UserRoleDeleteCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UserRoleDeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRoleRepository.DeleteRoleAsync(request.Id);
            //if (userRoles == null)
            //    return false;

           // _unitOfWork.UserRoleRepository.Remove(userRoles);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
