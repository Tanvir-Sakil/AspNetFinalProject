using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Staffs.Queries;

namespace DevSkill.Inventory.Application.Features.Staffs.Handlers
{
    public class GetStaffByIdQueryHandler : IRequestHandler<GetStaffByIdQuery, StaffDto>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetStaffByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StaffDto> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
        {
            var staff = await _unitOfWork.StaffRepository.GetByIdAsync(request.Id);

            if (staff == null)
                return null;

            return new StaffDto
            {
                Id = staff.Id,
                StaffCode = staff.StaffCode,
                EmployeeName = staff.EmployeeName,
                Phone = staff.Phone,
                Email = staff.Email,
                Address = staff.Address,
                NID = staff.Nid,
                JoiningDate = staff.JoiningDate,
                IsActive = staff.IsActive
            };
        }
    }
}
