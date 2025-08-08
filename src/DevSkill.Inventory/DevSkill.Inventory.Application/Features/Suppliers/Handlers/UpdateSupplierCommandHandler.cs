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
    public class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public UpdateSupplierHandler(IApplicationUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(request.Id);
            if (supplier == null) return false;

            supplier.SupplierCode = request.SupplierCode;
            supplier.SupplierName = request.SupplierName;
            supplier.Mobile = request.Mobile;
            supplier.Company = request.Company;
            supplier.Email = request.Email;
            supplier.Address = request.Address;
            supplier.OpeningBalance = request.OpeningBalance;
            return await _unitOfWork.SaveChangesAsync()>0;
           // return supplier.Id;
                 

        }
    }
}
