using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Companies.Queries;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Handlers
{
    public class SearchProductQueryHandler : IRequestHandler<SearchProductQuery, List<ProductDropDownDto>>
    {
        private readonly IProductRepository _productRepository;

        public SearchProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDropDownDto>> Handle(SearchProductQuery request, CancellationToken cancellationToken)
        {
            var priceType = await _productRepository
                .GetPriceTypeBySaleTypeIdAsync(request.SaleTypeId, cancellationToken);

            if (priceType == null)
                return new List<ProductDropDownDto>();

            var products = await _productRepository.SearchByNameAsync(request.Query);

            var result = products
                .Select(p => new ProductDropDownDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Stock = p.Stock,
                    UnitPrice = priceType == "MRP" ? p.MRP :
                                priceType == "Wholesale" ? p.WholesalePrice :
                                0
                })
                .ToList();

            return result;
        }

    }
}
