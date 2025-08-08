using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace DevSkill.Inventory.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>,IEntity<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Staff Employee { get; set; }
        public ApplicationRole Role { get; set; }
    }
}