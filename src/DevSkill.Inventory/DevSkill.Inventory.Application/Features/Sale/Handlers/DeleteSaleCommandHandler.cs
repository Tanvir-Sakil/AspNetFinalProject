using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Sale.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Sale.Handlers
{
    public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public DeleteSaleCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.SalesRepository.GetByIdAsync(request.Id);
            if (sale == null) return false;

            _unitOfWork.SalesRepository.Remove(sale);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
