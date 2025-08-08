using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.CashAccounts.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Queries
{
    public class GetCashAccountListQuery : DataTables, 
        IRequest<(IList<Domain.Entities.CashAccount> Data, int Total, int TotalDisplay)>, 
        IGetCashAccountListQuery
    {
        public string? Name { get; set; }

        public bool? IsActive { get; set; }


        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo {  get; set; }

        public decimal? BalanceFrom { get; set; }

        public decimal? BalanceTo { get; set; }

        public string? OrderBy { get; set; }
        
    }
}
