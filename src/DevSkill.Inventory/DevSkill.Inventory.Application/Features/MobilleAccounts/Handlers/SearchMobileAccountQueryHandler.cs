using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Repositories;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Handlers
{
    public class SearchMobileAccountQueryHandler : IRequestHandler<SearchMobileAccountQuery, List<SearchMobileAccountDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        private readonly IMobileAccountRepository _mobileAccountRepository;

        public SearchMobileAccountQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMobileAccountRepository mobileAccountRepository)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mobileAccountRepository = mobileAccountRepository;
        }

        public async Task<List<SearchMobileAccountDto>> Handle(SearchMobileAccountQuery request, CancellationToken cancellationToken)
        {
            var mobileAccounts = _mobileAccountRepository.SearchByName(request.Query);

            var result = mobileAccounts
                .Select(c => new SearchMobileAccountDto
                {
                    Id = c.Id,
                    AccountName = c.AccountName
                })
                .ToList();

             return  await Task.FromResult(result);
        }
    }
}
