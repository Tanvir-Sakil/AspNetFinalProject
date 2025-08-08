using DevSkill.Inventory.Application.Features.Account.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/BalanceAccount")]
    public class BalanceAccountController : Controller
    {
        private readonly IMediator _mediator;

        public BalanceAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAccountTypes")]
        public async Task<IActionResult> GetAccountTypes()
        {
            var accountTypes = await _mediator.Send(new GetAccountTypesQuery());
            var response = accountTypes.Select(t => new { id = t.Id, text = t.Name });
            return Ok(response);
        }
    }
}
