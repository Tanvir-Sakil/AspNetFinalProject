using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Handlers
{
    public class AddUserRoleCommandHandler : IRequestHandler<AddUserRoleCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddUserRoleCommandHandler(IApplicationUnitOfWork unitOfWork) => _applicationUnitOfWork = unitOfWork;

        public async Task<bool> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
        {
            var role = new UserRole(request.RoleName, request.CompanyName);
            await _applicationUnitOfWork.UserRoleRepository.AddRoleAsync(role);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
