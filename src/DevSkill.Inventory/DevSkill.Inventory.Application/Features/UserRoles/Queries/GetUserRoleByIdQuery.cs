using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Queries
{
    public class GetUserRoleByIdQuery : IRequest<UserRole>
    {
        public Guid Id { get; set; }
    }
}
