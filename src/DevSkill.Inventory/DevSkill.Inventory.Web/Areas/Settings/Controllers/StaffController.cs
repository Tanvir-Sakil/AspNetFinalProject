using DevSkill.Inventory.Application.Features.Staffs.Commands;
using DevSkill.Inventory.Application.Features.Staffs.Queries;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


public class StaffController : Controller
{
    private readonly IMediator _mediator;

    public StaffController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> GetStaffJsonData()
    {
        var result = await _mediator.Send(new GetStaffListQuery());
        return Json(new { data = result });
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddStaffCommand command)
    {
        var id = await _mediator.Send(command);
        return Json(new { success = true, id });
    }

    [HttpGet]
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
}

