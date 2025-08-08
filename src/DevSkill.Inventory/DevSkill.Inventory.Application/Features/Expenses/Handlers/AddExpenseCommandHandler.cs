using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Expenses.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Expenses.Handlers
{
    public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddExpenseCommandHandler(IApplicationUnitOfWork unitOfWork) => _applicationUnitOfWork = unitOfWork;

        public async Task<bool> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            await _applicationUnitOfWork.ExpenseRepository.AddAsync(expense);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
