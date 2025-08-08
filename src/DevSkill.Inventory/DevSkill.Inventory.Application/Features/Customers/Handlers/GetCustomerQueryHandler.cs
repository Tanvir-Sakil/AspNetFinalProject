using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Features.Companies.Queries;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Features.Customers.Handlers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomersQuery, (IList<Customer>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetCustomerQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<Customer>, int, int)> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CustomerRepository.GetPagedCustomerAsync(request);
        }
    }
}
