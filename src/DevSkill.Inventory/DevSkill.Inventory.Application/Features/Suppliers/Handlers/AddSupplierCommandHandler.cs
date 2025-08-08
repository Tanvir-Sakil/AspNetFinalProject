using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Suppliers.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Suppliers.Handlers
{
    public class AddSupplierHandler : IRequestHandler<AddSupplierCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddSupplierHandler(IApplicationUnitOfWork applicationUnitOfWork) => _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<bool> Handle(AddSupplierCommand request, CancellationToken cancellationToken)
        {
            var newSupplierCode = await _applicationUnitOfWork.SupplierRepository.GenerateNextSupplierCodeAsync();
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                SupplierCode = newSupplierCode,
                SupplierName = request.SupplierName,
                Mobile = request.Mobile,
                Company = request.Company,
                Email = request.Email,
                Address = request.Address,
                OpeningBalance = request.OpeningBalance,
                Status = "Active"
            };
            await _applicationUnitOfWork.SupplierRepository.AddAsync(supplier);
            return await _applicationUnitOfWork.SaveChangesAsync()>0;
            //return supplier.Id;
        }
    }
}
