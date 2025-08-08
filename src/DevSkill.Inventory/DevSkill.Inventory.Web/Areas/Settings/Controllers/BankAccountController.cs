using DevSkill.Inventory.Application.Features.Account.Queries;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevSkill.Inventory.Web.Areas.Settings.Models;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{

    [Area("Settings")]
    [Route("Settings/BankAccount")]
    public class BankAccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BankAccountController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var data = await _mediator.Send(new GetAllBankAccountsQuery());
        //    return Ok(data);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromForm] BankAccount model)
        //{
        //    await _mediator.Send(new CreateBankAccountCommand(model));
        //    return Ok(new { success = true });
        //}

        //[HttpPut]
        //public async Task<IActionResult> Update([FromForm] BankAccount model)
        //{
        //    await _mediator.Send(new UpdateBankAccountCommand(model));
        //    return Ok(new { success = true });
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _mediator.Send(new DeleteBankAccountCommand(id));
        //    return Ok(new { success = true });
        //}

        [HttpPost("GetBankAccountListJson")]
        public async Task<IActionResult> GetBankAccountListJson([FromBody] BankListModel model)
        {
            var query = _mapper.Map<GetBankAccountListQuery>(model.SearchItem);

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
                    accountName = c.AccountName,
                    accountNo = c.AccountNo,
                    bankName = c.BankName,
                    branchName = c.BranchName,
                    openingBalance = c.OpeningBalance,
                    currentBalance = c.CurrentBalance,
                    isActive = c.IsActive,
                    createdDate = c.CreatedDate.ToString("yyyy-MM-dd")
                })
            };

            return Json(result);
        }

        [HttpGet("GetBankAccount/{id}")]
        public async Task<IActionResult> GetBankAccount(Guid id)
        {
            var bankAccount = await _mediator.Send(new GetBankAccountByIdQuery { Id = id });
            if (bankAccount == null)
                return NotFound();

            var command = new BankAccountUpdateCommand
            {
                Id = bankAccount.Id,
                AccountName = bankAccount.AccountName,
                AccountNo = bankAccount.AccountNo,
                BankName = bankAccount.BankName,
                BranchName = bankAccount.BranchName,
                Balance = bankAccount.OpeningBalance,
                CurrentBalance = bankAccount.CurrentBalance,
                IsActive = bankAccount.IsActive
            };

            return PartialView("~/Areas/Settings/Views/Shared/_EditBankAccountModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddBankAccountCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool created = await _mediator.Send(command);
            if (!created)
                return BadRequest(new { success = false, message = "Failed to add bank account" });

            return Ok(new { success = true });
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] BankAccountUpdateCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool updated = await _mediator.Send(command);
            if (!updated)
                return NotFound(new { success = false, message = "Bank Account not found" });

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(BankAccountDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound(new { success = false, message = "Bank Account not found" });

            return Ok(new { success = true });
        }

        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _mediator.Send(new GetAccountsByTypeNameQuery { AccountTypeName = "Bank" });
            var response = accounts.Select(a => new { id = a.Id, name = a.Name });
            return Ok(response);
        }
    }
}
