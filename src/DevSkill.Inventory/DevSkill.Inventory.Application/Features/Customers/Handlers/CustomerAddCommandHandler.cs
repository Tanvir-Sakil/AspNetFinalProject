using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.CustomerLedger.Commands;
using DevSkill.Inventory.Domain.Utilities;

namespace DevSkill.Inventory.Application.Features.Customers.Handlers
{
    public class CustomerAddCommandHandler : IRequestHandler<CustomerAddCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMediator _mediator;
        private readonly IFileUploader _fileUploader;

        public CustomerAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, 
            IMediator mediator, IFileUploader fileUploader)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mediator = mediator;
            _fileUploader = fileUploader;
        }

        public async Task<Guid> Handle(CustomerAddCommand request, CancellationToken cancellationToken)
        {

            var NextID = await _applicationUnitOfWork.CustomerRepository.GenerateNextCustomerCodeAsync();

            string savedPath = null;
            string imageFileName = null;

            if (request.ImageFile != null && !string.IsNullOrEmpty(request.ImageFileName))
            {
                var extension = System.IO.Path.GetExtension(request.ImageFileName);
                imageFileName = $"{Guid.NewGuid()}{extension}";
                savedPath = await _fileUploader.UploadAsync(request.ImageFile, imageFileName, "uploads/customers");
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                ImagePath = savedPath,
                CustomerID = NextID,
                Name = request.Name,
                CompanyName = request.CompanyName,
                MobileNumber = request.MobileNumber,
                Address = request.Address,
                Email = request.Email,
                OpeningBalance = request.OpeningBalance,
                IsActive = true
            };

            if (!string.IsNullOrEmpty(imageFileName))
            {
                string uploader = "TanvirSakil";
                string imageType = "Products";
                string processorName = "TanvirSakil";

                var fullImagePath = $"customers/{imageFileName}";
                await _applicationUnitOfWork.SqsRepository.SendResizeRequestAsync(fullImagePath, uploader, imageType, processorName);
            }

            await _applicationUnitOfWork.CustomerRepository.AddAsync(customer);
            await _applicationUnitOfWork.SaveAsync();


            if (request.OpeningBalance > 0)
            {
                await _mediator.Send(new AddCustomerLedgerCommand
                {
                    CustomerId = customer.Id,
                    InvoiceNo = null,
                    Particulars = "Opening Balance",
                    Total = request.OpeningBalance,
                    Discount = 0,
                    Vat = 0,
                    Paid = 0,
                    Date = DateTime.Now,
                    SourceType = "OpeningBalance",
                    SourceId = customer.Id
                });
            }

            return customer.Id;
        }
    }
}
