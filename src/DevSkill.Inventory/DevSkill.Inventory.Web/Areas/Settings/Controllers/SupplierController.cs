using DevSkill.Inventory.Application.Features.Suppliers.Commands;
using DevSkill.Inventory.Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IMediator _mediator;
        public SupplierController(IMediator mediator) => _mediator = mediator;

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllSuppliersQuery());
            return Json(new { data = result });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddSupplierCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(command);
            return result ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UpdateSupplierCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(command);
            return result ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteSupplierCommand { Id = id });
            return result ? Ok(new { success = true }) : BadRequest(new { success = false });
        }
    }
}
