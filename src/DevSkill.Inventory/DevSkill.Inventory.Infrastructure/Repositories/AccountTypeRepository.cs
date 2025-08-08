using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountTypeDto>> GetAllActiveAsync(CancellationToken cancellationToken)
        {
            return await _context.AccountTypes
                .Where(t => t.IsActive)
                .Select(t => new AccountTypeDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
