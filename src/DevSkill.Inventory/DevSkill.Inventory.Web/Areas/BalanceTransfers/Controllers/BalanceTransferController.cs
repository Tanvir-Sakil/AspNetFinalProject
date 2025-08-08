using AutoMapper;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Command;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Application.Features.Sale.Queries;
using DevSkill.Web.Areas.BalanceTransfers.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.BalanceTrasfers.Controllers
{
    [Area("BalanceTransfers")]
    [Route("BalanceTransfers")]
    public class BalanceTransfersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BalanceTransfersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddBalanceTransferCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok(new { message = "Transfer successful" });

            return BadRequest("Transfer failed");
        }
        [HttpPost("GetTransfers")]
        public async Task<IActionResult> GetTransfers([FromBody] GetAllBalanceTransferQuery query)
        {
            query.OrderBy = query.FormatSortExpression(
                "FromAccountType", "ToAccountType"
            );

            var (data, total, totalDisplay) = await _mediator.Send(query);

            var response = data.Select(a => new
            {
                id = a.Id,
                createdAt = a.CreatedAt,
                fromAccountName = a.FromAccountName,
                toAccountName = a.ToAccountName,
                amount = a.Amount,
                note = a.Note
            }).ToList();

            return Json(new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = response
            });
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new BalanceTransferDeleteCommand { Id = id };
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { message = "Balance transfer deleted successfully." });

            return NotFound(new { message = "Balance transfer not found." });
        }
    }
}
