using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

public class GetExpenseListQueryHandler : IRequestHandler<GetExpenseListQuery, (IList<Expense> Data, int Total, int TotalDisplay)>
{
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public GetExpenseListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
    {
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<(IList<Expense> Data, int Total, int TotalDisplay)> Handle(GetExpenseListQuery request, CancellationToken cancellationToken)
{
        
        return await _applicationUnitOfWork.ExpenseRepository.GetPagedExpensesAsync(request);
    }
}
