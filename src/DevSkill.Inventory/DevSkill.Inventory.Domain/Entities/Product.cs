namespace DevSkill.Inventory.Domain.Entities
{
    public class Product :IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Code { get; set; } 

        public string Name { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid? UnitId { get; set; }
        public Unit Unit { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal MRP { get; set; }

        public decimal WholesalePrice { get; set; }

        public int Stock { get; set; }
        public int LowStock { get; set; }

        public int Damage { get; set; }

        public string? ImagePath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
