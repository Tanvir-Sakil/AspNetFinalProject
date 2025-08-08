using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Suppliers.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Suppliers.Handlers
{
    public class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public DeleteSupplierHandler(IApplicationUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(request.Id);
            if (supplier == null) return false;

            _unitOfWork.SupplierRepository.Remove(supplier);
            return await _unitOfWork.SaveChangesAsync()>0;

            //return supplier.Id;
        }
    }
}
