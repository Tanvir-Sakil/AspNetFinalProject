using AutoMapper;
using DevSkill.Inventory.Application.Features.Expenses.Commands;
using DevSkill.Inventory.Application.Features.Expenses.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/Expense")]
    public class ExpenseController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ExpenseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult IndexSP()
        {
            return View();
        }

        [HttpPost("GetExpenseListJson")]
        public async Task<IActionResult> GetExpenseListJson([FromBody] ExpenseListModel model)
        {
            var query = _mapper.Map<GetExpenseListQuery>(model.SearchItem);
            if (!string.IsNullOrWhiteSpace(model.SearchItem.Status))
            {
                if (model.SearchItem.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                    query.IsActive = true;
                else if (model.SearchItem.Status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                    query.IsActive = false;
            }
            query.OrderBy = query.FormatSortExpression("Name", "IsActive", "CreateDate", "Id");

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
                    name = c.Name,
                    isActive = c.IsActive,
                    createdDate = c.CreatedDate.ToString("yyyy-MM-dd")
                })
            };

            return Json(result);
        }


        [HttpGet("GetExpense/{id}")]
        public async Task<IActionResult> GetExpense(Guid id)
        {
            var expense = await _mediator.Send(new GetExpenseByIdQuery { Id = id });
            if (expense == null)
                return NotFound();

            var command = new ExpenseUpdateCommand
            {
                Id = expense.Id,
                Name = expense.Name,
                IsActive = expense.IsActive
            };

            return PartialView("~/Areas/Admin/Views/Shared/_EditExpenseModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddExpenseCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Edit([FromBody] ExpenseUpdateCommand command)
        {

            Console.WriteLine("EDIT CALLED: " + command.Id); 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            bool updated = await _mediator.Send(command);
            if (!updated)
            {
                return NotFound(new { success = false, message = "Expense not found" });
            }

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(ExpenseDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
