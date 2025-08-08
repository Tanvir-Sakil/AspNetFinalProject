using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Application.Features.Categories.Queries;

namespace DevSkill.Inventory.Application.Features.Categories.Handlers
{
    public class SearchCategoryQueryHandler : IRequestHandler<SearchCategoryQuery, List<SearchCategoryDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        private readonly ICategoryRepository _categoryRepository;

        public SearchCategoryQueryHandler(IApplicationUnitOfWork categoryOfWork, ICategoryRepository categoryRepository)
        {
            _applicationUnitOfWork = categoryOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<SearchCategoryDto>> Handle(SearchCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = _categoryRepository.SearchByName(request.Query);

            var result = categories
                .Select(c => new SearchCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return await Task.FromResult(result);
        }
    }
}
