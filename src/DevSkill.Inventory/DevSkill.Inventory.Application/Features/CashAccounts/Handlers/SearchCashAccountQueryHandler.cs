using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Repositories;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Handlers
{
    public class SearchCashAccountQueryHandler : IRequestHandler<SearchCashAccountQuery, List<SearchCashAccountDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        private readonly ICashAccountRepository _cashAccountRepository;

        public SearchCashAccountQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, ICashAccountRepository cashAccountRepository)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _cashAccountRepository = cashAccountRepository;
        }

        public async Task<List<SearchCashAccountDto>> Handle(SearchCashAccountQuery request, CancellationToken cancellationToken)
        {
            var cashAccounts = _cashAccountRepository.SearchByName(request.Query);

            var result = cashAccounts
                .Select(c => new SearchCashAccountDto
                {
                    Id = c.Id,
                    Name = c.AccountName
                })
                .ToList();

             return  await Task.FromResult(result);
        }
    }
}
