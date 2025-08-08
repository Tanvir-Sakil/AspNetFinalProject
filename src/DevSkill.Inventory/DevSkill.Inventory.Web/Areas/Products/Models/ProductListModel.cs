using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Products.Models
{
        public class ProductListModel : DataTables
        {
            public ProductSearchModel SearchItem { get; set; }
        }
}
