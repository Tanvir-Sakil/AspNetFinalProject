using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Enums;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Sale.Commands
{
    public class SaleUpdateCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNo { get; set; }
        public Guid CustomerID { get; set; }
        public Guid SalesType { get; set; }
        public string AccountType { get; set; }
        public string AccountNo { get; set; }
        public string Note { get; set; }
        public string Terms { get; set; }
        public decimal VAT { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<SaleItemDto> Items { get; set; }
    }
}
