using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Web.Areas.Customers.Models
{
    public class SalesInvoiceViewModel
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public string PaymentStatus { get; set; }

        public CompanyProfileViewDto CompanyProfile { get; set; }
    }
}
