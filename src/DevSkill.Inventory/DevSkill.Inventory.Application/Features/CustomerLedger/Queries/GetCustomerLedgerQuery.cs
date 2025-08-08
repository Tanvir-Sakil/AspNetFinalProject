using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CustomerLedger.Queries
{
    using MediatR;

    public class GetCustomerLedgerQuery : IRequest<CustomerLedgerViewDto>
    {
        public Guid CustomerId { get; set; }
        public string ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? ReportYear { get; set; }
    }

}
