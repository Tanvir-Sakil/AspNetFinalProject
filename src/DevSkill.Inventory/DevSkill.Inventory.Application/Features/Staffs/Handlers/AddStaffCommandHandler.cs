using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Staffs.Commands;

namespace DevSkill.Inventory.Application.Features.Staffs.Handlers
{
    public class AddStaffCommandHandler : IRequestHandler<AddStaffCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public AddStaffCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddStaffCommand request, CancellationToken cancellationToken)
        {
            var newStaffCode = await _unitOfWork.StaffRepository.GenerateNextStaffCodeAsync();

            var staff = new Staff
            {
                Id = Guid.NewGuid(),
                StaffCode = newStaffCode,
                EmployeeName = request.EmployeeName,
                DepartmentId = request.DepartmentId,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                JoiningDate = request.JoiningDate,
                Salary = request.Salary,
                Nid = request.Nid
            };

            await _unitOfWork.StaffRepository.AddAsync(staff);
            await _unitOfWork.SaveAsync();
            return staff.Id;
        }
    }

}
