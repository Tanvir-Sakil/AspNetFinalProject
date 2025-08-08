namespace DevSkill.Web.Areas.Customers.Models
{
    public class CustomerSearchModel
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public decimal? BalanceFrom { get; set; }

        public decimal BalanceTo { get; set; }

    }
}
