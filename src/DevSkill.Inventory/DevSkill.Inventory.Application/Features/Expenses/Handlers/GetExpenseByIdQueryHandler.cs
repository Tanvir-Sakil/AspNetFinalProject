using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Expenses.Handlers
{

    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, Expense>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetExpenseByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Expense> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ExpenseRepository.GetByIdAsync(request.Id);
        }
    }
}
