using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{
    public class GetUserRoleQuery : IRequest<string>
    {
        public Guid UserId { get; }

        public GetUserRoleQuery(Guid userId)
        {
            UserId = userId;
        }
    }

}
