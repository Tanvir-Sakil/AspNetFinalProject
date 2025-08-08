using DevSkill.Inventory.Application.Features.Staffs.Queries;
using DevSkill.Inventory.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.User.Controllers
{
    [Area("User")]
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("GetStaffJsonData")]
        public async Task<IActionResult> GetStaffJsonData()
        {
            var result = await _mediator.Send(new GetStaffListQuery());
            return Json(new { data = result });
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException("You can add delete functionality later.");
        }

    }

}
