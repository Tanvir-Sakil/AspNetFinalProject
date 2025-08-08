using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Suppliers.Queries
{
    public class GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDto>> { }
}
