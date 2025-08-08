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
    public class ExpenseUpdateCommandHandler : IRequestHandler<ExpenseUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public ExpenseUpdateCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ExpenseUpdateCommand request, CancellationToken cancellationToken)
        {
            var expense = await _unitOfWork.ExpenseRepository.GetByIdAsync(request.Id);
            if (expense == null)
                return false;

            expense.Name = request.Name;
            expense.IsActive = request.IsActive;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
