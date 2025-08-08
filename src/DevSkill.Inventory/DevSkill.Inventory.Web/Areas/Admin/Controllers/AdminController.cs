using DevSkill.Inventory.Application.Features.AccessSetUp.Commands;
using DevSkill.Inventory.Application.Features.AccessSetUp.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
public class AdminController : Controller
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> AssignPermissions(Guid userId)
    {
        var existingPermissions = await _mediator.Send(new GetPermissionsForUserQuery(userId));

        var model = new AssignPermissionViewModel
        {
            UserId = userId,
            ExistingPermissions = existingPermissions
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignPermissions(AssignPermissionViewModel model)
    {
        await _mediator.Send(new AssignPermissionsCommand(model.UserId, model.Permissions));

        TempData["Message"] = "Permissions saved!";
        return RedirectToAction("AssignPermissions", new { userId = model.UserId });
    }
}
