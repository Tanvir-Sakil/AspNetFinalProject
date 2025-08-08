using DevSkill.Inventory.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevSkill.Inventory.Web.Areas.Admin.ViewComponents
{
    public class UserMenuViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public UserMenuViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return View("Default", "Guest");

            var roleName = await _mediator.Send(new GetUserRoleQuery(Guid.Parse(userId)));
            return View("Default", roleName);
        }
    }
}
