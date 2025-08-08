
using DevSkill.Inventory.Application.Features.Staffs.Commands;
using DevSkill.Inventory.Application.Features.Staffs.Queries;
using DevSkill.Inventory.Application.Features.Users.Commands;
using DevSkill.Inventory.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DevSkill.Inventory.Web.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    [Route("UserManagement/User")]
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
            throw new NotImplementedException();
        }

        [HttpPost("GetUserListJsonData")]
        public async Task<IActionResult> GetGetUserListJsonData()
        {
            var (data, total, totalDisplay) = await _mediator.Send(new GetUserListQuery());

            var result = new
            {

                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = data.Select((c, index) => new
                {
                    id = c.Id,
                    userName = c.UserName,
                    companyName = c.CompanyName,
                    email = c.Email,
                    roleName = c.RoleName,
                    isActive = c.IsActive
                })
            };
            return Json(result);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery{ Id = id });
            if (user == null) return NotFound();

            var command = new UpdateUserCommand
            {
                Id = user.Id,
                UserName = user.UserName,
                RoleId = user.RoleId,
                IsActive = user.IsActive
            };

            return PartialView("_UpdateUserModal", command);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                TempData["Success"] = "User Updated Successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "User not found or Update failed.";
            return RedirectToAction("Index");
        }
    }
}
