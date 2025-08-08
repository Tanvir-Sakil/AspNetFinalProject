using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Expenses.Commands
{
    public class AddExpenseCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }=true;
    }
}
