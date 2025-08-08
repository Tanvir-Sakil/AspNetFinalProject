using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Users.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Users.Handlers
{
    public class UserUpdateCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public UserUpdateCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id);

            if (user == null)
                return false;

            user.Update(request.RoleId,request.UserName, request.IsActive);

            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
