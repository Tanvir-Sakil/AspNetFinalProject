using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Departments.Commands;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Handlers
{
    public class DepartmentUpdateCommandHandler : IRequestHandler<DepartmentUpdateCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public DepartmentUpdateCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DepartmentUpdateCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.Id);
            if (department == null)
                return false;

            department.Name = request.Name;
            department.IsActive = request.IsActive;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
