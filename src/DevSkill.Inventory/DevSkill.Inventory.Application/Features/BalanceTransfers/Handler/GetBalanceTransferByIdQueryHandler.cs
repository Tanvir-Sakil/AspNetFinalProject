using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Handler
{
    public class GetBalanceTranferByIdQueryHandler : IRequestHandler<GetBalanceTransferByIdQuery, BalanceTransfer>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetBalanceTranferByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<BalanceTransfer> Handle(GetBalanceTransferByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BalanceTransferRepository.GetByIdAsync(request.Id);
        }
    }
}
