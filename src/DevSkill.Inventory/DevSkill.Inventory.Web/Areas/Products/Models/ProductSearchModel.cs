namespace DevSkill.Inventory.Web.Areas.Products.Models
{
    public class ProductSearchModel
    {
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public decimal? MRPPriceFrom { get; set; }
        public decimal? MRPPriceTo { get; set; }

        public decimal? PurchasePriceFrom { get; set; }
        public decimal? PurchasePriceTo { get; set; }

        public decimal? WholeSalePriceFrom { get; set; }
        public decimal? WholeSalePriceTo { get; set; }

        public double? StockFrom { get; set; }

        public double StockTo { get; set; }
    }
}
