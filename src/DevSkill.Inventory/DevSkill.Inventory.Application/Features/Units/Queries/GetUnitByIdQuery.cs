using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Units.Queries
{
    public class GetUnitByIdQuery : IRequest<Domain.Entities.Unit>
    {
        public Guid Id { get; set; }
    }
}
