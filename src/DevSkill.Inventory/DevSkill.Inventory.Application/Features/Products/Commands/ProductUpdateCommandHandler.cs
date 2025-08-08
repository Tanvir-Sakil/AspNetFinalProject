using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Utilities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork  _applicationUnitOfWork;
        private readonly IFileUploader _fileUploader;

        public ProductUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IFileUploader fileUploader)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _fileUploader = fileUploader;
        }

        public async Task<bool> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(request.Id);

            if (product == null) return false;

            string savedPath = null;
            string imageFileName = null;

            if (request.ImageFile != null && !string.IsNullOrEmpty(request.ImageFileName))
            {
                var extension = System.IO.Path.GetExtension(request.ImageFileName);
                imageFileName = $"{Guid.NewGuid()}{extension}";
                savedPath = await _fileUploader.UploadAsync(request.ImageFile, imageFileName, "uploads/products");
            }
            product.Code = request.Code;
            product.Name = request.Name;
            product.CategoryId = request.CategoryId;
            product.UnitId = request.UnitId;
            product.PurchasePrice = request.PurchasePrice;
            product.MRP = request.MRP;
            product.WholesalePrice = request.WholesalePrice;
            product.Stock = request.Stock;
            product.LowStock = request.LowStock;
            product.Damage = request.Demage;

            if (!string.IsNullOrEmpty(imageFileName))
            {
                string uploader = "TanvirSakil";
                string imageType = "Products";
                string processorName = "TanvirSakil";

                var fullImagePath = $"products/{imageFileName}";
                await _applicationUnitOfWork.SqsRepository.SendResizeRequestAsync(fullImagePath, uploader, imageType, processorName);
            }

            _applicationUnitOfWork.ProductRepository.Update(product);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
