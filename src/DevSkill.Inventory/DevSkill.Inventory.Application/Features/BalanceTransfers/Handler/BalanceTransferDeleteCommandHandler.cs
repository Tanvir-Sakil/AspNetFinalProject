using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Command;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Handler
{
    public class BalanceTransferDeleteCommandHandler : IRequestHandler<BalanceTransferDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public BalanceTransferDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(BalanceTransferDeleteCommand request, CancellationToken cancellationToken)
        {
            var balanceTransfer = await _applicationUnitOfWork.BalanceTransferRepository.GetByIdAsync(request.Id);
            if (balanceTransfer == null)
                return false;

            _applicationUnitOfWork.BalanceTransferRepository.Remove(balanceTransfer);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
