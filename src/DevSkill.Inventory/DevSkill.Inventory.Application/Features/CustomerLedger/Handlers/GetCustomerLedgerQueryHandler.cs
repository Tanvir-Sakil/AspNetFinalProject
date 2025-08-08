using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Features.CustomerLedger.Queries;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Repositories;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CustomerLedger.Handlers
{
    public class GetCustomerLedgerQueryHandler : IRequestHandler<GetCustomerLedgerQuery, CustomerLedgerViewDto>
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ICustomerLedgerRepository _ledgerRepo;
//
        public GetCustomerLedgerQueryHandler(ICustomerRepository customerRepo,
            ICustomerLedgerRepository ledgerRepo
            )
        {
            _customerRepo = customerRepo;
            _ledgerRepo = ledgerRepo;
        }

        public async Task<CustomerLedgerViewDto> Handle(GetCustomerLedgerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepo.GetByIdAsync(request.CustomerId);
            if (customer == null) return null;

            var year = request.ReportYear ?? DateTime.Now.Year;
            var ledgerItems = await _ledgerRepo.GetCustomerLedgerAsync(request.CustomerId,request.ReportType,request.StartDate,
                request.EndDate,request.Month,request.Year,request.ReportYear);

            return new CustomerLedgerViewDto
            {
                CustomerName = customer.Name,
                Address = customer.Address,
                ContactNo = customer.MobileNumber,
                ReportYear = year,
                Transactions = ledgerItems
            };
        }
    }
}
