using DevSkill.Inventory.Application.Features.Suppliers.Commands;
using DevSkill.Inventory.Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    [Route("UserManagement/Supplier")]
    public class SupplierController : Controller
    {
        private readonly IMediator _mediator;
        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllSuppliersQuery());
            return Json(new {
                recordsTotal = result.Count(),
                recordsFiltered = result.Count(),
                data = result 
            });
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddSupplierCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(command);
            return result ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromBody] UpdateSupplierCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(command);
            return result ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteSupplierCommand { Id = id });
            return result ? Ok(new { success = true }) : BadRequest(new { success = false });
        }
    }
}
