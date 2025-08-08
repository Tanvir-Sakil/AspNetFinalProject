using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Domain.Features.Products.Queries
{
    public interface IGetProductsQuery : IDataTables
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

        public double? StockTo { get; set; }

        public string? OrderBy { get; set; }
    }
}
