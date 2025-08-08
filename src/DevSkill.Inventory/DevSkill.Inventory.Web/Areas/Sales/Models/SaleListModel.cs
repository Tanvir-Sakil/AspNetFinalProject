using System.Web;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Services;

namespace DevSkill.Inventory.Web.Areas.Sales.Models
{
    public class SaleListModel : DataTables
    {
        public SaleSearchModel SearchItem { get; set; } 
    }
}
