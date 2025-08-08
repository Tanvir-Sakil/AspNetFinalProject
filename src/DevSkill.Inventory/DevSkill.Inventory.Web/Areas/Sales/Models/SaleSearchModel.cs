namespace DevSkill.Inventory.Web.Areas.Sales.Models
{
    public class SaleSearchModel
    {
        public string? CustomerName { get; set; }
        public decimal? TotalAmountFrom { get; set; }
        public decimal? TotalAmountTo { get; set; }
        public decimal? PaidAmountFrom { get; set; }
        public decimal? PaidAmountTo { get; set; }
        public decimal? DueAmountFrom { get; set; }
        public decimal? DueAmountTo { get; set; }

    }
}
