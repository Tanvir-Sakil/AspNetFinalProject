using AutoMapper;
using DevSkill.Inventory.Application.Features.Departments.Queries;
using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Settings.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/Role")]
    public class UserRoleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserRoleController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        //[HttpGet("")]
        //public IActionResult Index()
        //{
        //    return View();
       // }

        [HttpGet("")]
        public IActionResult IndexSP()
        {
            return View();
        }
        [HttpPost("GetUserRoleListJson")]
        public async Task<IActionResult> GetUserRoleListJson([FromBody] UserRoleListModel model)
        {
            var query = _mapper.Map<GetUserRoleListQuery>(model.SearchItem);
            if (!string.IsNullOrWhiteSpace(model.SearchItem.Status))
            {
                if (model.SearchItem.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                    query.IsActive = true;
                else if (model.SearchItem.Status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                    query.IsActive = false;
            }
            query.OrderBy = query.FormatSortExpression("Name","CompanyName", "IsActive", "CreateDate", "Id");

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
                    companyName = c.CompanyName,
                    isActive = c.IsActive,
                    createdDate = c.CreatedDate.ToString()
                })
            };

            return Json(result);
        }

        [HttpGet("GetAllUserRole")]
        public async Task<IActionResult> GetAllUserRole()
        {
            var result = await _mediator.Send(new GetAllUserRolesQuery());
            if (result == null)
                return NotFound();

            var response = result.Select(
                x => new
                {
                    id = x.Id,
                    name = x.Name
                }
              );

            return Ok(response);
        }
        [HttpGet("GetUserRole/{id}")]
        public async Task<IActionResult> GetUserRole(Guid id)
        {
            var userRole = await _mediator.Send(new GetUserRoleByIdQuery { Id = id });
            if (userRole == null)
                return NotFound();

            var command = new UserRoleUpdateCommand
            {
                Id = userRole.Id,
                RoleName = userRole.Name,
               // CompanyName = userRole.CompanyName,
                IsActive = userRole.IsActive
            };

            return PartialView("~/Areas/Settings/Views/Shared/_EditUserRoleModal.cshtml", command);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddUserRoleCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }


        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] UserRoleUpdateCommand command)
        {

            Console.WriteLine("EDIT CALLED: " + command.Id); //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            bool updated = await _mediator.Send(command);
            if (!updated)
            {
                return NotFound(new { success = false, message = "UserRole not found" });
            }

            return Ok(new { success = true });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(UserRoleDeleteCommand command)
        {
            bool deleted = await _mediator.Send(command);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
