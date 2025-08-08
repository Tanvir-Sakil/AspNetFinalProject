using System;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Utilities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductAddCommandHandler : IRequestHandler<ProductAddCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IFileUploader _fileUploader;

        public ProductAddCommandHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            IFileUploader fileUploader)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            
            _fileUploader = fileUploader;
        }

        public async Task<bool> Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            if (request.UnitId == null && !string.IsNullOrEmpty(request.NewUnitName))
            {
                var existingUnit = await _applicationUnitOfWork.UnitRepository
                    .GetByNameAsync(request.NewUnitName.Trim());

                if (existingUnit != null)
                {
                    request.UnitId = existingUnit.Id;
                }
                else
                {
                    var newUnit = new Domain.Entities.Unit
                    {
                        Id = Guid.NewGuid(),
                        Name = request.NewUnitName.Trim(),
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _applicationUnitOfWork.UnitRepository.AddAsync(newUnit);
                    await _applicationUnitOfWork.SaveAsync();
                    request.UnitId = newUnit.Id;
                }
            }

            string savedPath = null;
            string imageFileName = null;

            if (request.ImageFile != null && !string.IsNullOrEmpty(request.ImageFileName))
            {
                var extension = System.IO.Path.GetExtension(request.ImageFileName);
                imageFileName = $"{Guid.NewGuid()}{extension}";
                savedPath = await _fileUploader.UploadAsync(request.ImageFile, imageFileName, "uploads/products");
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Name = request.Name,
                CategoryId = request.CategoryId,
                UnitId = request.UnitId,
                PurchasePrice = request.PurchasePrice,
                MRP = request.MRP,
                WholesalePrice = request.WholesalePrice,
                Stock = 0,
                LowStock = request.LowStock,
                Damage = 0,
                ImagePath = savedPath,
                CreatedDate = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(imageFileName))
            {
                string uploader = "TanvirSakil"; 
                string imageType = "Products";
                string processorName = "TanvirSakil";

                var fullImagePath = $"products/{imageFileName}"; 
                await _applicationUnitOfWork.SqsRepository.SendResizeRequestAsync(fullImagePath, uploader, imageType, processorName);
            }

            await _applicationUnitOfWork.ProductRepository.AddAsync(product);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
