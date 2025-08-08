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
    public class AddDamageProductCommandHandler : IRequestHandler<AddDamageProductCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public AddDamageProductCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddDamageProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null) return false;

            product.Damage += request.Quantity;
            product.Stock -= request.Quantity;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
