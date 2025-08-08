using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Repositories;

namespace DevSkill.Inventory.Application.Features.Units.Handlers
{
    public class SearchUnitQueryHandler : IRequestHandler<SearchUnitQuery, List<SearchUnitDto>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        private readonly IUnitRepository _unitRepository;

        public SearchUnitQueryHandler(IApplicationUnitOfWork unitOfWork, IUnitRepository unitRepository)
        {
            _unitOfWork = unitOfWork;
            _unitRepository = unitRepository;
        }

        public async Task<List<SearchUnitDto>> Handle(SearchUnitQuery request, CancellationToken cancellationToken)
        {
            var units = _unitRepository.SearchByName(request.Query);

            var result = units
                .Select(c => new SearchUnitDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

             return  await Task.FromResult(result);
        }
    }
}
