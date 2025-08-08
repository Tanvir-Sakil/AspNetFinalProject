using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class ImageResizeQueue : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string OriginalPath { get; set; } = null!;
        public string? ResizedPath { get; set; }
        public bool IsProcessed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
