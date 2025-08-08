using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Repositories;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Handlers
{
    public class SearchBankAccountQueryHandler : IRequestHandler<SearchBankAccountQuery, List<SearchBankAccountDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        private readonly IBankAccountRepository _bankAccountRepository;

        public SearchBankAccountQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IBankAccountRepository bankAccountRepository)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<List<SearchBankAccountDto>> Handle(SearchBankAccountQuery request, CancellationToken cancellationToken)
        {
            var bankAccounts = _applicationUnitOfWork.BankAccountRepository.SearchByName(request.Query);

            var result = bankAccounts
                .Select(c => new SearchBankAccountDto
                {
                    Id = c.Id,
                    AccountName = c.AccountName
                })
                .ToList();

             return  await Task.FromResult(result);
        }
    }
}
