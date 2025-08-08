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
    public class DepartmentDeleteCommandHandler : IRequestHandler<DepartmentDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public DepartmentDeleteCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DepartmentDeleteCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.Id);
            if (department == null)
                return false;

            _unitOfWork.DepartmentRepository.Remove(department);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
