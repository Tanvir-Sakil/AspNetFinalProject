using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Queries
{
    public class GetAllDepartmentsQuery : IRequest<List<Department>>
    {
    }
}
