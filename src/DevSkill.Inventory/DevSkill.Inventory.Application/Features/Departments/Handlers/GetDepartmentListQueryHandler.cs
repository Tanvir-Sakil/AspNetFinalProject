using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

public class GetDepartmentListQueryHandler : IRequestHandler<GetDepartmentListQuery, (IList<Department> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetDepartmentListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<Department> Data, int Total, int TotalDisplay)> Handle(GetDepartmentListQuery request, CancellationToken cancellationToken)
{
        
        return await _applicationUnitOfWork.DepartmentRepository.GetPagedDepartmentsAsync(request);
    }
}
