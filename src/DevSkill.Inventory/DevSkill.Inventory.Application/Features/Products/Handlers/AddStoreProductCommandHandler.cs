using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Handlers
{
    public class AddStoreProductCommandHandler : IRequestHandler<AddStoreProductCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public AddStoreProductCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddStoreProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null) return false;

            product.Stock += request.Quantity;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
