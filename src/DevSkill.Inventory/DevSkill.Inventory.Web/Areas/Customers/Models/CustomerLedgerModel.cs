using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web.Areas.Customers.Models
{
    public class CustomerLedgerModel
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
