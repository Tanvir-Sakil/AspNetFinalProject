using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class UserRole : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string? CompanyName { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public string ConcurrencyStamp { get; private set; }

        public UserRole(string roleName, string? companyName)
        {
            Id = Guid.NewGuid();
            Name = roleName;
            CompanyName = companyName;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }

        public UserRole(Guid id, string roleName, bool isActive, string concurrencyStamp)
        {
            Id = id;
            Name = roleName; 
            IsActive = isActive;
            ConcurrencyStamp = concurrencyStamp;
        }

        public void Update(string roleName, bool isActive)
        {
            Name = roleName;
            IsActive = isActive;
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }

    }


}
