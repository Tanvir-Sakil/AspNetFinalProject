using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, (IList<Category> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetCategoryListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<Category> Data, int Total, int TotalDisplay)> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
{
        
        return await _applicationUnitOfWork.CategoryRepository.GetPagedCategoriesAsync(request);
    }
}
