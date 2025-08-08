using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Users.Queries;

namespace DevSkill.Inventory.Application.Features.Users.Handlers
{
    public class GetUseristQueryHandler : IRequestHandler<GetUserListQuery, (IList<UserDto> Data, int Total, int TotalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetUseristQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<(IList<UserDto> Data, int Total, int TotalDisplay)> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {

            return await _applicationUnitOfWork.UserRepository.GetPagedUserAsync(request);
        }
    }
}
