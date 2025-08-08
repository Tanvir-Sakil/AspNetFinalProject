using AutoMapper;
using DevSkill.Inventory.Application.Features.Account.Queries;
using DevSkill.Inventory.Application.Features.MobileAccounts.Commands;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/MobileAccount")]
    public class MobileAccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MobileAccountController(IMediator mediator, IMapper mapper)
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
        //    var data = await _mediator.Send(new GetAllMobileAccountsQuery());
        //    return Ok(data);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromForm] MobileAccount model)
        //{
        //    await _mediator.Send(new CreateMobileAccountCommand(model));
        //    return Ok(new { success = true });
        //}

        //[HttpPut]
        //public async Task<IActionResult> Update([FromForm] MobileAccount model)
        //{
        //    await _mediator.Send(new UpdateMobileAccountCommand(model));
        //    return Ok(new { success = true });
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _mediator.Send(new DeleteMobileAccountCommand(id));
        //    return Ok(new { success = true });
        //}

        [HttpPost("GetMobileAccountListJson")]
        public async Task<IActionResult> GetMobileAccountListJson([FromBody] MobileListModel model)
        {
            var query = _mapper.Map<GetMobileAccountListQuery>(model.SearchItem);

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
                    ownerName = c.OwnerName,
                    openingBalance = c.OpeningBalance,
                    currentBalance = c.CurrentBalance,
                    isActive = c.IsActive,
                    createdDate = c.CreatedDate.ToString("yyyy-MM-dd")
                })
            };

            return Json(result);
        }

        [HttpGet("GetMobileAccount")]
        public async Task<IActionResult> GetMobileAccount(Guid id)
        {
            var mobileAccount = await _mediator.Send(new GetMobileAccountByIdQuery { Id = id });
            if (mobileAccount == null)
                return NotFound();

            var command = new MobileAccountUpdateCommand
            {
                Id = mobileAccount.Id,
                AccountName = mobileAccount.AccountName,
                AccountNo = mobileAccount.AccountNo,
                OwnerName = mobileAccount.OwnerName,
                Balance = mobileAccount.OpeningBalance,
                CurrentBalance = mobileAccount.CurrentBalance,
                IsActive = mobileAccount.IsActive
            };

            return PartialView("~/Areas/Settings/Views/Shared/_EditMobileAccountModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddMobileAccountCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool created = await _mediator.Send(command);
            if (!created)
                return BadRequest(new { success = false, message = "Failed to add mobile account" });

            return Ok(new { success = true });
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] MobileAccountUpdateCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool updated = await _mediator.Send(command);
            if (!updated)
                return NotFound(new { success = false, message = "Mobile Account not found" });

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(MobileAccountDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound(new { success = false, message = "Mobile Account not found" });

            return Ok(new { success = true });
        }

        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _mediator.Send(new GetAccountsByTypeNameQuery { AccountTypeName = "Mobile" });
            var response = accounts.Select(a => new { id = a.Id, name = a.Name });
            return Ok(response);
        }
    }
}
