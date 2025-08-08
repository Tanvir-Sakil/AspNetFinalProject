using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class AddCompanyModel
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required, Range(1.00, 5.00), RegularExpression("^\\d+(\\.\\d{1,2})?$", ErrorMessage = "Rating should be given 2 decimal places")]
        public double Rating { get; set; }
    }
}
