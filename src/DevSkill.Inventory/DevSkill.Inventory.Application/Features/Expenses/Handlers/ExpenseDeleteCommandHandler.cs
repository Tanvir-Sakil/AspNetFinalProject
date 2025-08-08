using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Expenses.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Expenses.Handlers
{
    public class ExpenseDeleteCommandHandler : IRequestHandler<ExpenseDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public ExpenseDeleteCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ExpenseDeleteCommand request, CancellationToken cancellationToken)
        {
            var expense = await _unitOfWork.ExpenseRepository.GetByIdAsync(request.Id);
            if (expense == null)
                return false;

            _unitOfWork.ExpenseRepository.Remove(expense);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
