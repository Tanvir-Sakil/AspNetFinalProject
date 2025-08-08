using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Handlers
{
    public class UnitDeleteCommandHandler : IRequestHandler<UnitDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public UnitDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(UnitDeleteCommand request, CancellationToken cancellationToken)
        {
            var unit = await _applicationUnitOfWork.UnitRepository.GetByIdAsync(request.Id);
            if (unit == null)
                return false;

            _applicationUnitOfWork.UnitRepository.Remove(unit);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
