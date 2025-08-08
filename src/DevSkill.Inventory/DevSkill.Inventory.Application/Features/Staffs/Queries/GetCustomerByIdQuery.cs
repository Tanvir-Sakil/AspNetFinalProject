using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Staffs.Queries
{
    public class GetStaffByIdQuery : IRequest<StaffDto>
    {
        public Guid Id { get; set; }
    }
}
