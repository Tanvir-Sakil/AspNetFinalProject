using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Handlers
{
    public class CategoryDeleteCommandHandler : IRequestHandler<CategoryDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public CategoryDeleteCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                return false;

            _unitOfWork.CategoryRepository.Remove(category);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
