using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
    public class GetSaleTypesQueryHandler : IRequestHandler<GetSaleTypesQuery, List<SaleTypeDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetSaleTypesQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<SaleTypeDto>> Handle(GetSaleTypesQuery request, CancellationToken cancellationToken)
        {
            var saleTypes = await _applicationUnitOfWork.SaleTypesRepository.GetAllAsync();

            var result = saleTypes.Select(s => new SaleTypeDto
            {
                Id = s.Id,
                Name = s.PriceName
            }).ToList();

            return result;
        }
    }
}
