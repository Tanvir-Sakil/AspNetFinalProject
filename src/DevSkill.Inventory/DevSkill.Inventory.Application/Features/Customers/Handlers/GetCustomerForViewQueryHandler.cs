using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Customers.Handlers
{
    public class GetCustomerForViewQueryHandler : IRequestHandler<GetCustomerForViewQuery, CustomerViewDto>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetCustomerForViewQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerViewDto> Handle(GetCustomerForViewQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetCustomerByIdWithSalesAsync(request.Id, cancellationToken);

            if (customer == null)
                return null;

            var model = new CustomerViewDto
            {
                Id = customer.Id,
                CustomerID = customer.CustomerID,
                Name = customer.Name,
                CompanyName = customer.CompanyName,
                MobileNumber = customer.MobileNumber,
                Address = customer.Address,
                Email = customer.Email,
                OpeningBalance = customer.OpeningBalance,
                ImagePath = customer.ImagePath,
                IsActive = customer.IsActive,
                Sales = customer.Sales.Select(s => new SalesInvoiceViewDto
                {
                    Id = s.Id,
                    InvoiceNo = s.InvoiceNo,
                    Date = s.Date,
                    PaymentStatus = s.PaymentStatus
                }).ToList()
            };

            return model;
        }
    }
}
