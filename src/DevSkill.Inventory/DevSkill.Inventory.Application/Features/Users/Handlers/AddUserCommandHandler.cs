using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Users.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Users.Handlers
{
        public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
        {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public AddUserCommandHandler(IApplicationUnitOfWork unitOfWork)
            {
                 _applicationUnitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _applicationUnitOfWork.UserRepository.CreateUserAsync(
                            request.Password,
                            request.EmployeeId,
                            request.RoleId);

                await _applicationUnitOfWork.SaveAsync();

                return true;
            }
        }
    }

