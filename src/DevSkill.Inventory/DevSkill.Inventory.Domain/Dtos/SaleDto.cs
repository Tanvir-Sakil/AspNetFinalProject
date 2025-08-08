using System;
using System.Collections.Generic;
using DevSkill.Inventory.Domain.Enums;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }

        public Guid CustomerID { get; set; }              
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerAddress { get; set; }
        public Guid SalesType { get; set; }               
        public string SaleTypeName { get; set; }        
        public decimal VAT {  get; set; }
        public decimal Discount { get; set; }

        public decimal NetTotal { get; set; }

        public string AccountType { get; set; }

        public string AccountNo { get; set; }

        public string Note { get; set; }

        public string Terms { get; set; }
        
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        public string PaymentStatus { get; set; }   


        public ICollection<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
        public ICollection<PaymentItemDto> PaymentItems { get; set; } = new List<PaymentItemDto>();

        public CompanyProfileViewDto CompanyProfileView { get; set; }
    }
}
