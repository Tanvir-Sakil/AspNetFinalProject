using System.Web;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Services;

namespace DevSkill.Web.Areas.Customers.Models
{
    public class CustomerListModel : DataTables
    {
        public CustomerSearchModel SearchItem { get; set; } 
    }
}
