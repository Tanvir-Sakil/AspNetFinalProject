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
using DevSkill.Inventory.Application.Features.Users.Queries;

namespace DevSkill.Inventory.Application.Features.Users.Handlers
{

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetUserByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
             return await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id);
        }
    }
}
