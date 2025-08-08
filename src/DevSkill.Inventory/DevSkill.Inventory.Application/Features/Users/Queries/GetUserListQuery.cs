using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Features.UserRoles.Queries;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Features.Users.Queries;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{
    public class GetUserListQuery : DataTables, IRequest<(IList<UserDto> Data, int Total, int TotalDisplay)>, IGetUserListQuery
    {
        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo { get; set; }

        public string? OrderBy { get; set; }
    }
}
