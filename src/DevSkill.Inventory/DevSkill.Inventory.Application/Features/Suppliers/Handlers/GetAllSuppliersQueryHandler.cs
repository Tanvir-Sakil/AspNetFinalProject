using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Suppliers.Queries;

namespace DevSkill.Inventory.Application.Features.Suppliers.Handlers
{
    public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetAllSuppliersHandler(IApplicationUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _unitOfWork.SupplierRepository.GetAllAsync();
            return suppliers.Select(s => new SupplierDto
            {
                Id = s.Id,
                SupplierCode = s.SupplierCode,
                SupplierName = s.SupplierName,
                Mobile = s.Mobile,
                Company = s.Company,
                Address = s.Address,
                Status = s.Status
            });
        }
    }
}
