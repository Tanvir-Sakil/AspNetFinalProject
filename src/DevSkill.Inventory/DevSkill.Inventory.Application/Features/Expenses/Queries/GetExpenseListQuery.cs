using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Expenses.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Expenses.Queries
{
    public class GetExpenseListQuery : DataTables, IRequest<(IList<Expense> Data, int Total, int TotalDisplay)>, IGetExpenseListQuery
    {
        public string? Name { get; set; }

        public bool? IsActive { get; set; }


        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo {  get; set; }

        public string? OrderBy { get; set; }
       
    }
}
