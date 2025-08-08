using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CompanyProfileRepository : Repository<CompanyProfile, Guid>,ICompanyProfileRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyProfileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<CompanyProfile> GetCompanyProfileAsync()
        {
            return await _context.CompanyProfiles.FirstOrDefaultAsync();
        }
    }
}
