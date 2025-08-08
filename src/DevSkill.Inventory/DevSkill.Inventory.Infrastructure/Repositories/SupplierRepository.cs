using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class SupplierRepository : Repository<Supplier,Guid>, ISupplierRepository
    {
        public readonly ApplicationDbContext _applicationDbContext;
        public SupplierRepository(ApplicationDbContext context) : base(context)
        {
            _applicationDbContext = context;

        }

        public async Task<string> GenerateNextSupplierCodeAsync()
        {
            var lastSupplier = await _applicationDbContext.Suppliers
                           .OrderByDescending(s => s.SupplierCode)
                           .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastSupplier != null && !string.IsNullOrEmpty(lastSupplier.SupplierCode))
            {
                var numberPart = lastSupplier.SupplierCode.Replace("S-INV", "");
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"S-INV{nextNumber:D5}";
        }
    }
}
