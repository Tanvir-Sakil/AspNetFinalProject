using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

public class GetUnitListQueryHandler : IRequestHandler<GetUnitListQuery, (IList<DevSkill.Inventory.Domain.Entities.Unit> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetUnitListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<DevSkill.Inventory.Domain.Entities.Unit> Data, int Total, int TotalDisplay)> Handle(GetUnitListQuery request, CancellationToken cancellationToken)
    {
        
        return await _applicationUnitOfWork.UnitRepository.GetPagedUnitsAsync(request);
    }
}
