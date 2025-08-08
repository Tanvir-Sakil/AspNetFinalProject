using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Units.Handlers
{
    public class AddUnitCommandHandler : IRequestHandler<AddUnitCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddUnitCommandHandler(IApplicationUnitOfWork unitOfWork) => _applicationUnitOfWork = unitOfWork;

        public async Task<bool> Handle(AddUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = new Domain.Entities.Unit
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            await _applicationUnitOfWork.UnitRepository.AddAsync(unit);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
