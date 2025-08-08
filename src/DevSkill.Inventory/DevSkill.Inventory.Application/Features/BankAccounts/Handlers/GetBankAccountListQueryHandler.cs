using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

public class GetBankAccountListQueryHandler : IRequestHandler<GetBankAccountListQuery, (IList<BankAccount> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetBankAccountListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<DevSkill.Inventory.Domain.Entities.BankAccount> Data, int Total, int TotalDisplay)> Handle(GetBankAccountListQuery request, CancellationToken cancellationToken)
    {
        
        return await _applicationUnitOfWork.BankAccountRepository.GetPagedBankAccountsAsync(request);
    }
}
