using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Departments.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Handlers
{
    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AddDepartmentCommandHandler(IApplicationUnitOfWork unitOfWork) => _applicationUnitOfWork = unitOfWork;

        public async Task<bool> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            await _applicationUnitOfWork.DepartmentRepository.AddAsync(department);
            await _applicationUnitOfWork.SaveAsync();
            return true;
        }
    }

}
