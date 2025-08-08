namespace DevSkill.Inventory.Web.Areas.Customers.Models
{
    public class CustomerLedgerItem
   {
    public DateTime Date { get; set; }
    public string Invoice { get; set; }
    public string Particulars { get; set; }
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    public decimal Vat { get; set; }
    public decimal Paid { get; set; }
    public decimal Balance { get; set; }
   }

}
