using DevSkill.Inventory.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.AccessSetup.Controllers
{
    [Area("AccessSetup")]
    [Route("AccessSetup")]
    public class AccessSetupController : Controller
    {
        private readonly IMediator _mediator;

        public AccessSetupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    

        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            var query = new GetUserListQuery();
            var (data, total, totalDisplay) = await _mediator.Send(query);

            var result = data.Select(x => new
            {
                id = x.Id,        
                companyName = x.CompanyName,       
                userName = x.UserName,
                userType = x.RoleName,
                status = x.IsActive,
                createdDate = x.CreatedDate
            });

            return Json(new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = result
            });
        }
    }
}
