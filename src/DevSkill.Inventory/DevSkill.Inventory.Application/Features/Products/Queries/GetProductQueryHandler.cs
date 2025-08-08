using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductsQuery, (IList<Product>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;


        public GetProductQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<Product>, int, int)> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var (products, total, totalDisplay) = await _applicationUnitOfWork.ProductRepository.GetPagedProductsSPAsync(request);

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var fileName = System.IO.Path.GetFileName(product.ImagePath);
                    var s3Key = $"TanvirSakil/resized/resized_{fileName}";
                    var presignedUrl = _applicationUnitOfWork.S3Repository.GeneratePreSignedURL(s3Key);

                    product.ImagePath = presignedUrl;
                }
            }

            return (products, total, totalDisplay);
        }
    }
}
