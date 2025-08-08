using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Staffs.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Staffs.Handlers
{
    public class GetStaffListQueryHandler : IRequestHandler<GetStaffListQuery, List<StaffDto>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetStaffListQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<StaffDto>> Handle(GetStaffListQuery request, CancellationToken cancellationToken)
        {
            var list = await _unitOfWork.StaffRepository.GetAllAsync();

            return list.Select(x => new StaffDto
            {
                Id = x.Id,
                StaffCode = x.StaffCode,
                EmployeeName = x.EmployeeName,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                JoiningDate = x.JoiningDate,
                Salary = x.Salary,
                IsActive = x.IsActive
            }).ToList();
        }
    }

}
