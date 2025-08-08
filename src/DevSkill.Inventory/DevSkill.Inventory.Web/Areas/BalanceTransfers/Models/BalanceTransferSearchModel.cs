namespace DevSkill.Web.Areas.BalanceTransfers.Models
{
    public class BalanceTransferSearchModel
    {
        public string? FromAccountName { get; set; }

        public string? ToAccountName { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal? AmountFrom { get; set; }

        public decimal? AmountTo { get; set; }

    }
}
