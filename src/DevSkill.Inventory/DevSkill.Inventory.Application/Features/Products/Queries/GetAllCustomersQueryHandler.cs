using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Services;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Companies.Queries
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<Customer>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetAllCustomersQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var companies = await _applicationUnitOfWork.CustomerRepository.GetAllAsync();
            return companies.ToList();

        }
    }
}
