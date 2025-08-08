namespace DevSkill.Inventory.Web.Models
{
    public class LogTable
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        public string MachineName { get; set; }
        public string ThreadId { get; set; }
    }
}
