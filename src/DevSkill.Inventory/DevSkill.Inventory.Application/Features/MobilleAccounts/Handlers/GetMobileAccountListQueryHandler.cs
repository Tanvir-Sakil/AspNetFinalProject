using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

public class GetMobileAccountListQueryHandler : IRequestHandler<GetMobileAccountListQuery, (IList<DevSkill.Inventory.Domain.Entities.MobileAccount> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetMobileAccountListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<DevSkill.Inventory.Domain.Entities.MobileAccount> Data, int Total, int TotalDisplay)> Handle(GetMobileAccountListQuery request, CancellationToken cancellationToken)
    {
        
        return await _applicationUnitOfWork.MobileAccountRepository.GetPagedMobileAccountsAsync(request);
    }
}
