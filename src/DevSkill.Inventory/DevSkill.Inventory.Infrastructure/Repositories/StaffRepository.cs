using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StaffRepository : Repository<Staff, Guid>, IStaffRepository
    {
        public readonly ApplicationDbContext _applicationDbContext;
        public StaffRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<string> GenerateNextStaffCodeAsync()
        {
            var lastStaff = await _applicationDbContext.Staffs
       .OrderByDescending(s => s.StaffCode)
       .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastStaff != null && !string.IsNullOrEmpty(lastStaff.StaffCode))
            {
                var numberPart = lastStaff.StaffCode.Replace("E-INV", "");
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"E-INV{nextNumber:D5}";
        }


    }
}
