using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.AccessSetUp.Queries
{
    public class GetPermissionsForUserQuery : IRequest<List<string>>
    {
        public Guid UserId { get; set; }

        public GetPermissionsForUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
