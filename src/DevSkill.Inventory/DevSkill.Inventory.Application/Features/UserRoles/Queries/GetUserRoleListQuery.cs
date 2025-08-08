using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.UserRoles.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Queries
{
    public class GetUserRoleListQuery : DataTables, IRequest<(IList<RoleDto> Data, int Total, int TotalDisplay)>, IGetUserRoleListQuery
    {
        public string? RoleName { get; set; }

        public string? CompanyName { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo {  get; set; }

        public string? OrderBy { get; set; }
       
    }
}
