using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Handlers
{

    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Department>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetDepartmentByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Department> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DepartmentRepository.GetByIdAsync(request.Id);
        }
    }
}
