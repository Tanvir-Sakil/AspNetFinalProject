using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.Staffs.Commands;
using DevSkill.Inventory.Application.Features.Staffs.Queries;
using DevSkill.Inventory.Application.Features.Suppliers.Commands;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.UserManagement.Controllers
{

    [Area("UserManagement")]
    [Route("UserManagement/Staff")]
    public class StaffController : Controller
    {
        private readonly IMediator _mediator;

        public StaffController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("GetStaffJsonData")]
        public async Task<IActionResult> GetStaffJsonData()
        {
            var result = await _mediator.Send(new GetStaffListQuery());
            return Json(new {
                recordsTotal = result.Count,
                recordsFiltered = result.Count,
                data = result
            });
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddStaffCommand command)
        {
            var id = await _mediator.Send(command);
            return Json(new { success = true, id });
        }

        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var result = await _mediator.Send(new GetAllStaffQuery());
            if (result == null)
                return NotFound();

            var response = result.Select(
                x => new
                {
                    id = x.Id,
                    name = x.EmployeeName,

                }
              );

            return Ok(response);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteStaffCommand { Id = id });
            return result ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var staff = await _mediator.Send(new GetStaffByIdQuery { Id = id });
            if (staff == null) return NotFound();

            var command = new UpdateStaffCommand
            {
                Id = staff.Id,
                EmployeeName = staff.EmployeeName,
                Phone = staff.Phone,
                Email = staff.Email,
                Address = staff.Address,
                NID = staff.NID,
                JoiningDate = staff.JoiningDate,
                IsActive = staff.IsActive
            };

            return PartialView("_UpdateStaffModal", command);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateStaffCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                TempData["Success"] = "Staff Updated Successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Staff not found or Update failed.";
            return RedirectToAction("Index");
        }
    }
}

