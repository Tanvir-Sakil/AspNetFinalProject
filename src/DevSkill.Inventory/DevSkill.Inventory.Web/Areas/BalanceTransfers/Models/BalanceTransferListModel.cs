using System.Web;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Services;

namespace DevSkill.Web.Areas.BalanceTransfers.Models
{
    public class BalanceTransferListModel : DataTables
    {
        public BalanceTransferSearchModel SearchItem { get; set; } 
    }
}
