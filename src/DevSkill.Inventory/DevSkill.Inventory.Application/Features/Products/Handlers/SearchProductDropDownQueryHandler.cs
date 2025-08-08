using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Handlers
{
    public class SearchProductDropdownQueryHandler : IRequestHandler<SearchProductDropdownQuery, List<SearchProductDropdownDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public SearchProductDropdownQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<SearchProductDropdownDto>> Handle(SearchProductDropdownQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ProductRepository.SearchDropdownProductsAsync(request.Query, cancellationToken);
        }
    }
}
