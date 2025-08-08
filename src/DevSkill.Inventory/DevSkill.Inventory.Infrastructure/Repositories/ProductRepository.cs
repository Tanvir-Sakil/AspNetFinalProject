using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Features.Units.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public readonly ApplicationDbContext _dbContext;

        protected ISqlUtility SqlUtility { get; private set; }

        public ProductRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
            SqlUtility = new SqlUtility(_dbContext.Database.GetDbConnection());
        }

        public async Task<(IList<Product> Data, int Total, int TotalDisplay)> GetPagedProductsSPAsync(IGetProductsQuery request)
        {
            Expression<Func<Product, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                filter = filter.AndAlso(c => c.Name.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                filter = filter.AndAlso(c => c.Category.Name.Contains(request.CategoryName));
            }

            if (request.MRPPriceFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.MRP >= request.MRPPriceFrom.Value);
            }

            if (request.MRPPriceTo.HasValue)
            {
                filter = filter.AndAlso(c => c.MRP <= request.MRPPriceTo.Value);
            }

            if (request.PurchasePriceFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.PurchasePrice >= request.PurchasePriceFrom.Value);
            }

            if (request.PurchasePriceTo.HasValue)
            {
                filter = filter.AndAlso(c => c.PurchasePrice <= request.PurchasePriceTo.Value);
            }

            if (request.WholeSalePriceFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.WholesalePrice >= request.MRPPriceFrom.Value);
            }

            if (request.WholeSalePriceTo.HasValue)
            {
                filter = filter.AndAlso(c => c.WholesalePrice <= request.MRPPriceTo.Value);
            }

            if (request.StockFrom.HasValue)
            {
                filter = filter.AndAlso(c => c.Stock >= request.StockFrom.Value);
            }

            if (request.StockTo.HasValue)
            {
                filter = filter.AndAlso(c => c.Stock <= request.StockTo.Value);
            }

          
            if (!string.IsNullOrWhiteSpace(request.Search.Value))
            {
                string searchTerm = request.Search.Value.Trim();

                filter = filter.AndAlso(c =>
                    c.Name.Contains(searchTerm) ||
                    c.Category.Name.Contains(searchTerm) ||
                    c.Unit.Name.Contains(searchTerm)
                );
            }

            return await GetDynamicAsync(
                filter,
                request.OrderBy,
                q => q.Include(p => p.Category).Include(p => p.Unit),
                request.PageIndex,
                request.PageSize,
                isTrackingOff: true
            );
        }



        public async Task<List<Product>> SearchByNameAsync(string query, int limit = 20)
        {
            return _dbContext.Set<Product>()
                .Where(c => c.Name.Contains(query))
                .OrderBy(c => c.Name)
                .Take(limit)
                .ToList();
        }

        public async Task<string?> GetPriceTypeBySaleTypeIdAsync(Guid saleTypeId, CancellationToken cancellationToken)
        {
            return await _dbContext.SaleTypes
                .Where(x => x.Id == saleTypeId)
                .Select(x => x.PriceName)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductDto> GetProductBySaleTypeAsync(Guid productId, Guid saleTypeId)
        {
            var saleType = await _dbContext.SaleTypes.FindAsync(saleTypeId);

            var product = await _dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Stock = p.Stock,
                    UnitPrice = saleType.PriceName == "MRP" ? p.MRP : p.WholesalePrice
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<Product> GetProductWithDetailsAsync(Guid id)
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<SearchProductDropdownDto>> SearchDropdownProductsAsync(string query, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products
                .Where(p => p.Name.Contains(query) || p.Code.Contains(query))
                .Select(p => new SearchProductDropdownDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code
                })
                .ToListAsync(cancellationToken);

            return products;
        }



    }
}
