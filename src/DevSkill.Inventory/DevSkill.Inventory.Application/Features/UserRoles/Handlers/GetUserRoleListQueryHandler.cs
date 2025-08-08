using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Dtos;

public class GetUserRoleListQueryHandler : IRequestHandler<GetUserRoleListQuery, (IList<RoleDto> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetUserRoleListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<RoleDto> Data, int Total, int TotalDisplay)> Handle(GetUserRoleListQuery request, CancellationToken cancellationToken)
{
        
        return await _applicationUnitOfWork.UserRoleRepository.GetPagedUserRolesAsync(request);
    }
}
