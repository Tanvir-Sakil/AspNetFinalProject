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
    public class AccountSearchRepository : IAccountSearchRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountSearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountTypeDto>> GetAccountsByTypeNameAsync(string typeName, CancellationToken cancellationToken)
        {
            typeName = typeName?.ToLower();

            return typeName switch
            {
                "bank" => await _context.BankAccounts
                    .Where(a => a.IsActive)
                    .Select(a => new AccountTypeDto { Id = a.Id, Name = a.AccountName })
                    .ToListAsync(cancellationToken),

                "cash" => await _context.CashAccounts
                    .Where(a => a.IsActive)
                    .Select(a => new AccountTypeDto { Id = a.Id, Name = a.AccountName })
                    .ToListAsync(cancellationToken),

                "mobile" => await _context.MobileAccounts
                    .Where(a => a.IsActive)
                    .Select(a => new AccountTypeDto { Id = a.Id, Name = a.AccountName })
                    .ToListAsync(cancellationToken),

                _ => new List<AccountTypeDto>()
            };
        }
    }
}
