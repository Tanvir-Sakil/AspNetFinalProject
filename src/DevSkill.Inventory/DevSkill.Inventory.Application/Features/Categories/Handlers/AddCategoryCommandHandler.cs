using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Handlers
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddCategoryCommandHandler(IApplicationUnitOfWork unitOfWork) => _applicationUnitOfWork = unitOfWork;

        public async Task<bool> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            await _applicationUnitOfWork.CategoryRepository.AddAsync(category);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
