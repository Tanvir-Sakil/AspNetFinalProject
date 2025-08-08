using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevSkill.Inventory.Domain.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public Guid EmployeeId { get; set; }

        public string RoleName { get; set; }
        public Guid RoleId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Staff Employee { get; set; }
        public UserRole Role { get; set; }

        public User(Guid id, Guid employeeId,Guid roleId ,string userName, bool isActive)
        {
            Id = id;
            EmployeeId = employeeId;
            UserName = userName;
            RoleId = roleId;
            IsActive = isActive;
        }

        public void Update(Guid roleId ,string userName, bool isActive)
        {
            RoleId = roleId;
            UserName = userName;
            IsActive = isActive;
        }
    }
}
