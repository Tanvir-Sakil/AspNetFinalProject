using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Units.Commands
{
    public class UnitDeleteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
