namespace DevSkill.Inventory.Web.Areas.Customers.Models
{
    public class CustomerViewModel
    {
        public Guid Id { get; set; }
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public decimal OpeningBalance { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }

        public List<SalesInvoiceViewModel> Sales { get; set; } = new();
    }
}
