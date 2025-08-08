using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Utilities;

namespace DevSkill.Inventory.Application.Features.Customers.Handlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IFileUploader _fileUploader;

        public UpdateCustomerCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IFileUploader fileUploader)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _fileUploader = fileUploader;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(request.Id);
            if (customer == null)
                return false;

            string savedPath = null;
            string imageFileName = null;

            if (request.ImageFile != null && !string.IsNullOrEmpty(request.ImageFileName))
            {
                var extension = System.IO.Path.GetExtension(request.ImageFileName);
                imageFileName = $"{Guid.NewGuid()}{extension}";
                savedPath = await _fileUploader.UploadAsync(request.ImageFile, imageFileName, "uploads/customers");
            }

            customer.CustomerID = request.CustomerID;
            customer.Name = request.Name;
            customer.MobileNumber = request.MobileNumber;
            customer.Email = request.Email;
            customer.Address = request.Address;
            customer.OpeningBalance = request.OpeningBalance;
            customer.IsActive = request.IsActive;
            customer.ImagePath = savedPath;

            if (!string.IsNullOrEmpty(imageFileName))
            {
                string uploader = "TanvirSakil";
                string imageType = "Products";
                string processorName = "TanvirSakil";

                var fullImagePath = $"products/{imageFileName}";
                await _applicationUnitOfWork.SqsRepository.SendResizeRequestAsync(fullImagePath, uploader, imageType, processorName);
            }

            _applicationUnitOfWork.CustomerRepository.Update(customer);

            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }

}
