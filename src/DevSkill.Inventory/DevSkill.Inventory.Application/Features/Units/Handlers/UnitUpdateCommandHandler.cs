using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Units.Handlers
{
    public class UnitUpdateCommandHandler : IRequestHandler<UnitUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public UnitUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(UnitUpdateCommand request, CancellationToken cancellationToken)
        {
            var unit = await _applicationUnitOfWork.UnitRepository.GetByIdAsync(request.Id);
            if (unit == null)
                return false;

            unit.Name = request.Name;
            unit.IsActive = request.IsActive;

            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
