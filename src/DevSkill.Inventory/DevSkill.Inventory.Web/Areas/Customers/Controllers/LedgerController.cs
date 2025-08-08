using Microsoft.AspNetCore.Mvc;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Web.Areas.Customers.Controllers
{
    using DevSkill.Inventory.Application.Features.CustomerLedger.Queries;
    using DevSkill.Inventory.Application.Features.Profile.Queries;
    using DevSkill.Inventory.Web.Areas.Customers.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Area("Customers")]
    [Route("Ledger")]
    public class LedgerController : Controller
    {
        private readonly IMediator _mediator;

        public LedgerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View(new CustomerLedgerModel());
        }

        [HttpGet("GenerateReport")]
        public async Task<IActionResult> GenerateReport(CustomerLedgerModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var companyProfile = await _mediator.Send(new GetCompanyProfileQuery());

            var query = new GetCustomerLedgerQuery
            {
                CustomerId = model.CustomerId,
                ReportType = model.ReportType,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Month = model.Month,
                Year = model.Year,
                ReportYear = model.ReportYear
            };

            var dto = await _mediator.Send(query);
            if (dto == null)
                return NotFound();

            var viewModel = new CustomerLedgerReportViewModel
            {
                CustomerName = dto.CustomerName,
                Address = dto.Address,
                ContactNo = dto.ContactNo,
                ReportYear = dto.ReportYear,
                CompanyProfile = companyProfile,
                Transactions = dto.Transactions.Select(t => new CustomerLedgerItem
                {
                    Date = t.Date,
                    Invoice = t.InvoiceNo,
                    Particulars = t.Particulars,
                    Total = t.Total,
                    Discount = t.Discount,
                    Vat = t.Vat,
                    Paid = t.Paid,
                    Balance = t.Balance
                }).ToList()
            };

            return View("ReportResult", viewModel); 
        }

    }
}
