using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.MobileAccounts.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class GetMobileAccountListQuery : DataTables, 
        IRequest<(IList<Domain.Entities.MobileAccount> Data, int Total, int TotalDisplay)>, 
        IGetMobileAccountListQuery
    {

        public string? AccountName { get; set; }
        public string? AccountNo { get; set; }
        public string? OwnerName { get; set; }
        public bool? IsActive { get; set; }


        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo {  get; set; }

        public decimal? BalanceFrom { get; set; }

        public decimal? BalanceTo { get; set; }

        public string? OrderBy { get; set; }
    }
}
