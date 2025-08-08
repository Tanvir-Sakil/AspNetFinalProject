using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Expenses.Commands
{
    public class ExpenseDeleteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
