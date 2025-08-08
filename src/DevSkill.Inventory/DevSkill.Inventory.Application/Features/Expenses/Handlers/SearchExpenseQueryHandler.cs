using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Application.Features.Expenses.Queries;

namespace DevSkill.Inventory.Application.Features.Expenses.Handlers
{
    public class SearchExpenseQueryHandler : IRequestHandler<SearchExpenseQuery, List<SearchExpenseDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public SearchExpenseQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<SearchExpenseDto>> Handle(SearchExpenseQuery request, CancellationToken cancellationToken)
        {
            var expenses = _applicationUnitOfWork.ExpenseRepository.SearchByName(request.Query);

            var result = expenses
                .Select(c => new SearchExpenseDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return await Task.FromResult(result);
        }
    }
}
