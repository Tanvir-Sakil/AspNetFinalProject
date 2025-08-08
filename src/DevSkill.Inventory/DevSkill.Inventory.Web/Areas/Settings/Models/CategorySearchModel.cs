using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Settings.Models
{
    public class CategorySearchModel
    {
        public string Name { get; set; }
        public string? Status { get; set; }

        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo { get; set; }
    }
}
