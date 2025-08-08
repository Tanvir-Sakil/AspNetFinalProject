using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.Companies.Queries;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
     public class SearchCustomerQueryHandler : IRequestHandler<SearchCustomerQuery, List<CustomerDropDownDto>>
     {
        private readonly ICustomerRepository _customerRepository;

        public SearchCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<List<CustomerDropDownDto>> Handle(SearchCustomerQuery request, CancellationToken cancellationToken)
        {
            var customers = _customerRepository.SearchByName(request.Query);

            var result = customers
                .Select(c => new CustomerDropDownDto
                {
                    Id = c.Id,
                    Name = c.Name
                    
                })
                .ToList();

            return Task.FromResult(result);
        }

        
    }
}
