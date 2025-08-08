using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MediatR;
using DevSkill.Inventory.Application.Features.CashAccounts.Commands;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using AutoMapper;
using DevSkill.Inventory.Application.Features.Account.Queries;
using DevSkill.Inventory.Web.Areas.Settings.Models;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/CashAccount")]
    public class CashAccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CashAccountController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("GetCashAccountListJson")]
        public async Task<IActionResult> GetCashAccountListJson([FromBody] CashListModel model)
        {
            var query = _mapper.Map<GetCashAccountListQuery>(model.SearchItem);

            query.OrderBy = query.FormatSortExpression("Name", "Balance", "CurrentBalance", "CreatedDate", "Id");
            query.Start = model.Start;
            query.Length = model.Length;

            var (data, total, totalDisplay) = await _mediator.Send(query);

            var result = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = data.Select((c, index) => new
                {
                    serialNumber = index + 1 + query.Start,
                    id = c.Id,
                    name = c.AccountName,
                    balance = c.Balance,
                    currentBalance = c.CurrentBalance,
                    isActive = c.IsActive,
                    createdDate = c.CreatedDate.ToString("yyyy-MM-dd")
                })
            };

            return Json(result);
        }

        [HttpGet("GetCashAccount")]
        public async Task<IActionResult> GetCashAccount(Guid id)
        {
            var cashAccount = await _mediator.Send(new GetCashAccountByIdQuery { Id = id });
            if (cashAccount == null)
                return NotFound();

            var command = new CashAccountUpdateCommand
            {
                Id = cashAccount.Id,
                Name = cashAccount.AccountName,
                Balance = cashAccount.Balance,
                CurrentBalance = cashAccount.CurrentBalance,
                IsActive = cashAccount.IsActive
            };

            return PartialView("~/Areas/Settings/Views/Shared/_EditCashAccountModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddCashAccountCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool created = await _mediator.Send(command);
            if (!created)
                return BadRequest(new { success = false, message = "Failed to add cash account" });

            return Ok(new { success = true });
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] CashAccountUpdateCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool updated = await _mediator.Send(command);
            if (!updated)
                return NotFound(new { success = false, message = "Cash Account not found" });

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(CashAccountDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound(new { success = false, message = "Cash Account not found" });

            return Ok(new { success = true });
        }

        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _mediator.Send(new GetAccountsByTypeNameQuery { AccountTypeName = "Cash"});
            var response = accounts.Select(a => new { id = a.Id, name = a.Name });
            return Ok(response);
        }
    }
}
