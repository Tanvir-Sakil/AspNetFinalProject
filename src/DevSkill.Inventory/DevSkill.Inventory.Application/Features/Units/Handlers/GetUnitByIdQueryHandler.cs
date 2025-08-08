using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Units.Handlers
{

    public class GetUnitByIdQueryHandler : IRequestHandler<GetUnitByIdQuery, Domain.Entities.Unit>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetUnitByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<Domain.Entities.Unit> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.UnitRepository.GetByIdAsync(request.Id);
        }
    }
}
