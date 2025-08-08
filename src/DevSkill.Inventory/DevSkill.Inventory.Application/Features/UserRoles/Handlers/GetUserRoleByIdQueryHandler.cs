using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Features.UserRoles.Handlers
{

    public class GetUserRoleByIdQueryHandler : IRequestHandler<GetUserRoleByIdQuery, UserRole>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetUserRoleByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRole> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
        {
             return await _unitOfWork.UserRoleRepository.GetRoleByIdAsync(request.Id);
        }
    }
}
