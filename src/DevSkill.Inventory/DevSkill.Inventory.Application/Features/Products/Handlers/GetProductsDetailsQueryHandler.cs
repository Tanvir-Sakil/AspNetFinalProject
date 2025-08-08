using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Handlers
{
    using MediatR;
    using DevSkill.Inventory.Domain.Dtos;
    using DevSkill.Inventory.Domain.Repositories;
    using DevSkill.Inventory.Application.Features.Products.Queries;
    using DevSkill.Inventory.Domain;

    public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, ProductDto>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetProductDetailsQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ProductRepository.GetProductBySaleTypeAsync(request.ProductId, request.SaleTypeId);
        }
    }

}
