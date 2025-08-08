using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Web.Areas.Customers.Models
{
    public class CustomerLedgerReportViewModel
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public int ReportYear { get; set; }
        public List<CustomerLedgerItem> Transactions { get; set; }
        public CompanyProfileViewDto CompanyProfile { get; set; }



        //public decimal GrandTotal => Transactions.Sum(t => t.Total);
        //public decimal TotalDiscount => Transactions.Sum(t => t.Discount);
        //public decimal TotalVat => Transactions.Sum(t => t.Vat);
        //public decimal TotalBalance => Transactions.Sum(t => t.Balance);
    }
}
