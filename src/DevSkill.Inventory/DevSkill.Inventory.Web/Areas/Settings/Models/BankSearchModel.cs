using System.Runtime.CompilerServices;
using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Settings.Models
{
    public class BankSearchModel
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatAt { get; set; }

    }
}
