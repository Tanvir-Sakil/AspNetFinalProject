namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class AssignPermissionViewModel
    {
        public Guid UserId { get; set; }  // Id of user to assign permissions

        //// Permission flags for Customer
        //public bool CustomerAdd { get; set; }
        //public bool CustomerEdit { get; set; }
        //public bool CustomerDelete { get; set; }

        //// Permission flags for Product
        //public bool ProductAdd { get; set; }
        //public bool ProductEdit { get; set; }
        //public bool ProductDelete { get; set; }

        //// Permission flags for Balance Transfer
        //public bool BalanceTransferAdd { get; set; }
        //public bool BalanceTransferEdit { get; set; }
        //public bool BalanceTransferDelete { get; set; }

        //// Permission flags for Sales
        //public bool SalesAdd { get; set; }
        //public bool SalesEdit { get; set; }
        //public bool SalesDelete { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();

        public List<string> ExistingPermissions { get; set; } = new();
    }
}
