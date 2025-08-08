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
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Application.Features.Staffs.Commands;

namespace DevSkill.Inventory.Application.Features.Customers.Handlers
{
    public class UpdateStaffCommandHandler : IRequestHandler<UpdateStaffCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public UpdateStaffCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<bool> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
        {
            var staff = await _applicationUnitOfWork.StaffRepository.GetByIdAsync(request.Id);
            if (staff == null)
                return false;

            staff.StaffCode = request.StaffCode;
            staff.EmployeeName = request.EmployeeName;
            staff.Phone = request.Phone;
            staff.Email = request.Email;
            staff.Address = request.Address;
            staff.JoiningDate = request.JoiningDate;
            staff.IsActive = request.IsActive;

            _applicationUnitOfWork.StaffRepository.Update(staff);

            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }

}
