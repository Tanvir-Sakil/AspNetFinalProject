using Microsoft.Identity.Client;

namespace DevSkill.Inventory.Web.Areas.Customers.Models
{
    public enum ResponseTypes
    {
        Success,
        Danger
    }
    public class ResponseModel
    {
        public string? Message {  get; set; }

        public ResponseTypes? Type { get; set; }
    }
}
