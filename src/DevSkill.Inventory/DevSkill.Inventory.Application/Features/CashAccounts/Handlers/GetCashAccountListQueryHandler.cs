using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

public class GetCashAccountListQueryHandler : IRequestHandler<GetCashAccountListQuery, (IList<DevSkill.Inventory.Domain.Entities.CashAccount> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetCashAccountListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<DevSkill.Inventory.Domain.Entities.CashAccount> Data, int Total, int TotalDisplay)> Handle(GetCashAccountListQuery request, CancellationToken cancellationToken)
    {
        
        return await _applicationUnitOfWork.CashAccountRepository.GetPagedCashAccountsAsync(request);
    }
}
