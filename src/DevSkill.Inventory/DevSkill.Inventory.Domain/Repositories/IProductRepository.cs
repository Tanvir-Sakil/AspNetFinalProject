using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        //public List<Product> GetLatest();

        //Task<(IList<Product>, int, int)> GetPagedProductsAsync(IGetProductsQuery request);

        Task<List<SearchProductDropdownDto>> SearchDropdownProductsAsync(string query, CancellationToken cancellationToken);

        Task<(IList<Product> Data, int Total, int TotalDisplay)> GetPagedProductsSPAsync(IGetProductsQuery request);

        Task<string?> GetPriceTypeBySaleTypeIdAsync(Guid saleTypeId, CancellationToken cancellationToken);

        Task<ProductDto> GetProductBySaleTypeAsync(Guid productId, Guid saleTypeId);

        Task<List<Product>> SearchByNameAsync(string query, int limit = 20);

        Task<Product> GetProductWithDetailsAsync(Guid id);
    }
}
