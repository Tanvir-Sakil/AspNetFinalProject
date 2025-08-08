using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IAccountTypeRepository
    {
        Task<List<AccountTypeDto>> GetAllActiveAsync(CancellationToken cancellationToken);
    }
}
