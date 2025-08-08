using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Staffs.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Staffs.Handlers
{
    public class DeleteStaffCommandHandler : IRequestHandler<DeleteStaffCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public DeleteStaffCommandHandler(IApplicationUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
        {
            var staff = await _unitOfWork.StaffRepository.GetByIdAsync(request.Id);
            if (staff == null) return false;

            _unitOfWork.StaffRepository.Remove(staff);
            return await _unitOfWork.SaveChangesAsync()>0;

            //return supplier.Id;
        }
    }
}
