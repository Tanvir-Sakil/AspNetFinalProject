using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class SaleTypeRepository : Repository<SaleType, Guid>, ISaleTypeRepository
    {
        public readonly ApplicationDbContext _dbContext;

        protected ISqlUtility SqlUtility { get; private set; }

        public SaleTypeRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
            SqlUtility = new SqlUtility(_dbContext.Database.GetDbConnection());
        }

        public async Task<List<SaleType>> GetAllSalesTypesAsync()
        {
            return await _dbContext.SaleTypes.ToListAsync();
        }


    }
}
