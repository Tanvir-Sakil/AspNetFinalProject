using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Customers.Queries;

namespace DevSkill.Inventory.Application.Features.Customers.Handlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetCustomerByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.Id);

            if (customer == null)
                return null;

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                CompanyName = customer.CompanyName,
                MobileNumber = customer.MobileNumber,
                Email = customer.Email,
                Address = customer.Address,
                OpeningBalance = customer.OpeningBalance,
                IsActive = customer.IsActive,
                ImagePath = customer.ImagePath
            };
        }
    }
}
