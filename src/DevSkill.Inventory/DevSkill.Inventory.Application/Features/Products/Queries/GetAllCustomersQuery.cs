using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class GetAllCustomersQuery : IRequest<List<Customer>>
    {
    }
}
