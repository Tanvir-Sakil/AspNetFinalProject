using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CustomerLedger.Commands
{
    public class AddCustomerLedgerCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public string InvoiceNo { get; set; }
        public string Particulars { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal Paid { get; set; }
        public DateTime Date { get; set; }
        public string SourceType { get; set; }
        public Guid? SourceId { get; set; }
    }
}
