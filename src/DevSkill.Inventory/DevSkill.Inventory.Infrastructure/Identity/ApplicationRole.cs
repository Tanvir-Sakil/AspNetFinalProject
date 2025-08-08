using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace DevSkill.Inventory.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<Guid>,IEntity<Guid>
    {
        public string? CompanyName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    }
}