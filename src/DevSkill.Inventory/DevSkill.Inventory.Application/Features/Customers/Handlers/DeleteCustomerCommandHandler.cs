using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Application.Features.Customers.Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.Id);

            if (customer == null)
                return false;

            _unitOfWork.CustomerRepository.Remove(customer);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
