using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DevSkill.Inventory.Web.Areas.Products.Models
{
    public class AddProductModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid CompanyId { get; set; }

        public DateTime PublishedAt { get; set; }

        public List<SelectListItem> CompanyList { get; set; }
    }
}
