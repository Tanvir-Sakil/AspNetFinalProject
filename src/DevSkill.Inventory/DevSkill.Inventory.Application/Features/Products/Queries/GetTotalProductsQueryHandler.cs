using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class GetTotalProductsQueryHandler : IRequestHandler<GetTotalProductsQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetTotalProductsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<int> Handle(GetTotalProductsQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ProductRepository.GetCountAsync();
        }
    }
}
