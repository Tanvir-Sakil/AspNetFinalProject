using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Users.Queries;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Users.Handlers
{
    public class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQuery, string>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetUserRoleQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
        {
            var userDto = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (userDto == null) return "Unknown";
           
            return userDto?.RoleName ?? "No Role";
        }
    }

}
